using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Omni.Core;
using UnityEngine;

public class Connection : DatabaseManager
{
    private static Connection instance;
    [SerializeField]
    private DatabaseType db = DatabaseType.MariaDb;
    [SerializeField]
    private string hostName = "localhost", dbName, userName, password;
    void Start()
    {
        instance = this;
        if(!NetworkManager.IsServerActive) return;
        var dbC = new DbCredentials();
        dbC.SetConnectionString(db, hostName, dbName, userName, password);
        
        SetDatabase(dbC);

        if (CheckConnection())
        {
            print("Conexão estabelecida com sucesso!");
        }
    }

    public static Task<DatabaseConnection> OpenConection()
    {
        return instance.GetConnectionAsync();
    }
}
