using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

public class VeSync
{
    private static bool ConsoleOutput = false;
    private const string BASE_URL = "https://smartapi.vesync.com";

    public PostLogin_Model LoginData { get; internal set; }

    public VeSync(string username, string password, bool Console_Output = false)
    {
        ConsoleOutput = Console_Output;
        Login_Model login;
        using (MD5 md5Hash = MD5.Create())
        {
            login = new Login_Model {
                account = username,
                password = Encryption.GetMd5HashHexStr(md5Hash, password)
            };
        }

        string payload = JsonConvert.SerializeObject(login);

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + "/vold/user/login");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = payload;

            streamWriter.Write(json);
        }

        HttpWebResponse httpResponse;
        string result;
        try
        {
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        catch(System.Net.WebException excpt)
        {
            if (excpt.Message.Contains("403"))
            {
                ConsoleLog("There was an error authorizing the account. Double check username and password!");
                throw new NotAuthorized();
            }
            else
            {
                throw excpt;
            }
        }

        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

        LoginData = JsonConvert.DeserializeObject<PostLogin_Model>(result);

        ConsoleLog("Result = {0}", result);
        
    }

    public Device[] GetDevices()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + "/vold/user/devices");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";
        httpWebRequest.Headers.Add("tk", LoginData.tk);
        httpWebRequest.Headers.Add("accountid", LoginData.accountID);


        HttpWebResponse httpResponse;
        string result;
        try
        {
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        catch (System.Net.WebException excpt)
        {
            if (excpt.Message.Contains("403"))
            {
                ConsoleLog("There was an error authorizing the account. Double check username and password!");
                throw new NotAuthorized();
            }
            else
            {
                throw excpt;
            }
        }

        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

        List<Device> devices = JsonConvert.DeserializeObject<List<Device>>(result);

        foreach(Device d in devices)
        {
            d.vs = this;
        }

        ConsoleLog("Result = {0}", result);
        return devices.ToArray();
    }

    public void turn_on(Device dvc)
    {
        SetDeviceState(dvc, DeviceState.On);
    }

    public void turn_off(Device dvc)
    {
        SetDeviceState(dvc, DeviceState.Off);
    }

    

    private void SetDeviceState(Device dvc, DeviceState ds)
    {
        string URL = BASE_URL + $"/v1/wifi-switch-1.3/{dvc.ID}/status/";
        switch (ds)
        {
            case DeviceState.Off:
                URL += "off";
                break;
            case DeviceState.On:
                URL += "on";
                break;
        }

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";
        httpWebRequest.Headers.Add("tk", LoginData.tk);
        httpWebRequest.Headers.Add("accountid", LoginData.accountID);


        HttpWebResponse httpResponse;
        string result;
        try
        {
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        catch (System.Net.WebException excpt)
        {
            if (excpt.Message.Contains("403"))
            {
                ConsoleLog("There was an error authorizing the account. Double check username and password!");
                throw new NotAuthorized();
            }
            else
            {
                throw excpt;
            }
        }

        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

        //List<Device> devices = JsonConvert.DeserializeObject<List<Device>>(result);

        ConsoleLog("Result = {0}", result);
    }

    

    private void ConsoleLog(string What, params string[] ps)
    {
        if (ConsoleOutput) 
        {
            string output = string.Format(What, ps);
            Console.WriteLine(output);
        }
    }
}

enum DeviceState
{
    On,
    Off
}
