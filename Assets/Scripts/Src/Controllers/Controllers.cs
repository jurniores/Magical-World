using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Omni.Core;
using Unity.VisualScripting;
using UnityEngine;

public class Controllers
{
    string table;

    public Controllers(string tableI)
    {
        table = tableI;
    }
    public async Task<int> Insert(Models model)
    {
        try
        {
            using var conn = await Connection.OpenConection();
            var builder = conn.GetBuilder(table);
            int result = await builder.InsertGetIdAsync<int>(model.ToModel());

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return 0;
        }
    }

    public async Task<T> GetId<T>(int id)
    {
        try
        {
            using var conn = await Connection.OpenConection();
            var builder = conn.GetBuilder(table);
            var result = await builder.Where(new { id }).FirstAsync<T>();

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }
    public async Task<int> UpdateAll(Models model)
    {
        try
        {
            using var conn = await Connection.OpenConection();
            var builder = conn.GetBuilder(table);
            var result = await builder.Where(new { model.id }).UpdateAsync(model.ToModel());
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return 0;
        }
    }

    public async Task<int> Update(int id, object model)
    {
        try
        {
            using var conn = await Connection.OpenConection();
            var builder = conn.GetBuilder(table);
            var result = await builder.Where(new { id }).UpdateAsync(model);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return 0;
        }
    }

    public async Task<int> DeleteId(int id)
    {
        try
        {
            using var conn = await Connection.OpenConection();
            var builder = conn.GetBuilder(table);
            var result = await builder.Where(new { id }).DeleteAsync();
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return 0;
        }
    }

    public async Task<T> Get<T>(object obj)
    {
        try
        {
            using var conn = await Connection.OpenConection();
            var builder = conn.GetBuilder(table);
            var result = await builder.Where(obj).FirstAsync<T>();

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }



}
