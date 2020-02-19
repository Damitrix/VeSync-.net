using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

public class Device
{
    /// <summary>
    /// Used to enable the TurnOn and TurnOff methods
    /// Not really a great way to do this.
    /// </summary>
    internal VeSync vs;
    
    [JsonProperty("deviceName")]
    public string Name { get; internal set; }
    
    [JsonProperty("deviceImg")]
    public string Img { get; internal set; }

    [JsonProperty("cid")]
    public string ID { get; internal set; }

    [JsonProperty("deviceStatus")]
    public string Status { get; internal set; }

    [JsonProperty("connectionType")]
    public string ConnectionType { get; internal set; }

    [JsonProperty("connectionStatus")]
    public string ConnectionStatus { get; internal set; }

    [JsonProperty("deviceType")]
    public string DeviceType { get; internal set; }

    [JsonProperty("model")]
    public string Model { get; internal set; }

    [JsonProperty("currentFirmVersion")]
    public string FirmwareVersion { get; internal set; }

    /// <summary>
    /// Powers on the device
    /// </summary>
    public void TurnOn()
    {
        vs.turn_on(this);
    }

    /// <summary>
    /// Powers off the device
    /// </summary>
    public void TurnOff()
    {
        vs.turn_off(this);
    }
}

