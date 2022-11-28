using System;
using System.Collections.Generic;

[Serializable]
public class ChartData
{
    public MetaData metaData = new();
    public List<FreeBox> freeBoxes = new();
}
[Serializable]
public class MetaData
{
    public string musicName = "";
    public string musicWriter = "";
    public string musicBPMText = "";
    public string artWriter = "";
    public string chartWriter = "";
    public string chartLevel = "";
    public string description = "";
    public List<BPM> BPM = new();
    public float offset = 0;
}
[Serializable]
public class BPM
{
    public float time = -1;
    public float bpm = -1;
}
[Serializable]
public class FreeBox
{
    public BoxEvents boxEvents = new();
}
[Serializable]
public class BoxEvents
{

}
[Serializable]
public class Event
{

}