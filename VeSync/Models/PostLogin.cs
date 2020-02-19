using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


public class PostLogin_Model
{
    [JsonProperty("tk")]
    public string Token { get; internal set; }
    
    [JsonProperty("accountID")]
    public string AccountID { get; internal set; }

    [JsonProperty("nickName")]
    public string NickName { get; internal set; }

    [JsonProperty("avatarIcon")]
    public string AvatarIconURL { get; internal set; }

    [JsonProperty("userType")]
    public int UserType { get; internal set; }

    [JsonProperty("acceptLanguage")]
    public string TOSLanguage { get; internal set; }

    [JsonProperty("termsStatus")]
    public bool TOS_Status { get; internal set; }
}

