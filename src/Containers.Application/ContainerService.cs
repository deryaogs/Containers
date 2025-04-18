﻿using Containers.Models;
using Microsoft.Data.SqlClient;

namespace Containers.Application;

public class ContainerService
{
    private string _connectionString;

    public ContainerService(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IEnumerable<Container> Containers()
    {
        List<Container> containers = [];
        const string queryString = "SELECT * FROM Containers";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var containerRow = new Container
                        {
                            ID = reader.GetInt32(0),
                            ContainerTypeId = reader.GetInt32(1),
                            isHazardous = reader.GetBoolean(2),
                            Name = reader.GetString(3)
                        };
                        containers.Add(containerRow);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return containers;
    }
}