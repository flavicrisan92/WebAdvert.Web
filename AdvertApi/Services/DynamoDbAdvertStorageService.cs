using AutoMapper;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using AdvertApi.Models.Flavius;

namespace AdvertApi.Services
{
    public class DynamoDbAdvertStorageService : IAdvertStorageService
    {
        private readonly IMapper _mapper;
        private readonly AmazonDynamoDBClient _client;

        public DynamoDbAdvertStorageService(IMapper mapper, IAmazonDynamoDB dynamoDb)
        {
            _mapper = mapper;
            _client = (AmazonDynamoDBClient)dynamoDb;
        }

        public async Task<string> Add(AdvertModel model)
        {
            var dbModel = _mapper.Map<AdvertDbModel>(model);
            dbModel.Id = new Guid().ToString();
            dbModel.CreationDateTime = DateTime.Now;
            dbModel.Status = AdvertStatus.Pending;

            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(dbModel);
                }
            }
            return dbModel.Id;
        }

        public async Task<bool> CheckHealthAsync()
        {
            using (var context = new DynamoDBContext(_client))
            {
                using(var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1))
                {
                    try
                    {

                        var tableData = await client.DescribeTableAsync("Adverts");
                        return tableData.Table.TableStatus == "active";
                    }
                    catch(Exception ex)
                    {
                        var aaa = ex;
                        throw;
                    }
                }
            }
        }

        public async Task Confirm(ConfirmAdvertModel model)
        {
            using (var context = new DynamoDBContext(_client))
            {
                var record = await context.LoadAsync<AdvertDbModel>(model.Id);
                if (record == null)
                {
                    throw new KeyNotFoundException($"A record with Id={model.Id} was not found");
                }

                if (model.Status == AdvertStatus.Active)
                {
                    record.FilePath = model.FilePath;
                    record.Status = AdvertStatus.Active;
                    await context.SaveAsync(record);
                }
                else
                {
                    await context.DeleteAsync(record);
                }
            }
        }
    }
}
