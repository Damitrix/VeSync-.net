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

    /// <summary>
    /// Cotains the responce from the VeSync servers
    /// </summary>
    public PostLogin_Model LoginData { get; internal set; }

    /// <summary>
    /// Creates a new connection to the API
    /// </summary>
    /// <param name="username">Username or email on the account</param>
    /// <param name="password">Password to the account</param>
    /// <param name="Console_Output">Enable additional information through the console</param>
    public VeSync(string username, string password, bool Console_Output = false)
    {
        //Set the console output variable
        ConsoleOutput = Console_Output;
        
        //Create a model for the login
        Login_Model login;

        using (MD5 md5Hash = MD5.Create())
        {
            //Create login credentials for serialization and format the password so it is in UTF-8 Hexidecimal
            login = new Login_Model {
                account = username,
                password = Encryption.GetMd5HashHexStr(md5Hash, password)
            };
        }

        //Get the login data in JSON format to pass to the server
        string payload = JsonConvert.SerializeObject(login);

        //Make the login request over POST
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
            //TRY TO GET A RESPONCE
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        catch(System.Net.WebException excpt)
        {
            //IF WE GET A 403 ERROR WE ARE NOT AUTHORIZED SO THROW AN ERROR OTHERWISE THROW THE EXCEPTION THROWN
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

        //GET THE RESPONCE AND MAKE IT A STRING
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

        //DESERIALIZE THE LOGIN AND STORE THE INFORMATION PROVIDED AS AN OBJECT
        LoginData = JsonConvert.DeserializeObject<PostLogin_Model>(result);

        //LOG THE FULL JSON RESULT IF ENABLED
        ConsoleLog("Result = {0}", result);        
    }


    /// <summary>
    /// Gets full list of device objects
    /// </summary>
    /// <returns>List of device objects on account</returns>
    public Device[] GetDevices()
    {        
        //Make request to get all of the devices on the account
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + "/vold/user/devices");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";
        httpWebRequest.Headers.Add("tk", LoginData.Token);
        httpWebRequest.Headers.Add("accountid", LoginData.AccountID);


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

        //FOR ALL OF THE DEVICES ADD THIS CLASS TO THE OBJECT SO THE USE OF TURNON and TURNOFF will work
        foreach(Device d in devices)
        {
            d.vs = this;
        }

        //LOG THE RAW JSON VALUE
        ConsoleLog("Result = {0}", result);

        //RETURN THE LIST OF DEVICES
        return devices.ToArray();
    }

    /// <summary>
    /// Turns on the device specified
    /// </summary>
    /// <param name="dvc">Device to power on</param>
    public void turn_on(Device dvc)
    {
        SetDeviceState(dvc, DeviceState.On);
    }

    /// <summary>
    /// Turns off the device specified
    /// </summary>
    /// <param name="dvc">Device to power off</param>
    public void turn_off(Device dvc)
    {
        SetDeviceState(dvc, DeviceState.Off);
    }

    

    /// <summary>
    /// Turns on or off the device
    /// </summary>
    /// <param name="dvc">Device to power</param>
    /// <param name="ds">Power State</param>
    private void SetDeviceState(Device dvc, DeviceState ds)
    {
        //CREATE POWER URL FROM DEVICE POWER STATE 
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

        //MAKE REQUEST
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";
        httpWebRequest.Headers.Add("tk", LoginData.Token);
        httpWebRequest.Headers.Add("accountid", LoginData.AccountID);


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
        
        ConsoleLog("Result = {0}", result);
    }

    

    /// <summary>
    /// Logs to console if output is enabled
    /// </summary>
    /// <param name="What">String </param>
    /// <param name="ps">String format specifiers</param>
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
