﻿namespace DotnetExam.Database.Mongo;

public class MongoDatabaseSettings
{
    public required string ConnectionString { get; set; }
    
    public required string DatabaseName { get; set; }
    
    public required string UsersCollectionName { get; set; }
}