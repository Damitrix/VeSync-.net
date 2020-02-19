using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


public class PostLogin_Model
{
    [JsonProperty]
    public string tk { get; internal set; }
    
    [JsonProperty]
    public string accountID { get; internal set; }

    [JsonProperty]
    public string nickName { get; internal set; }

    [JsonProperty]
    public string avatarIcon { get; internal set; }

    [JsonProperty]
    public int userType { get; internal set; }

    [JsonProperty]
    public string acceptLanguage { get; internal set; }

    [JsonProperty]
    public bool termsStatus { get; internal set; }
}

