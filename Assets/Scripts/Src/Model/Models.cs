using System.Text.Json.Serialization;
public class Models
{
    [JsonIgnore]
    public int id;
    public virtual object ToModel(){
        return null;
    }
}