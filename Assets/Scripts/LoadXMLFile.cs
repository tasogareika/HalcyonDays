using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class LoadXMLFile : MonoBehaviour {

    public List<TextAsset> xmlFiles;
    private TextAsset xmlRawFile;
    public static LoadXMLFile singleton;
    [HideInInspector] public int fileNo, BGMNo;
    [HideInInspector] public string data, label, nextXML, location, soundEffects, decision, commands;
    [HideInInspector] public List<string> textList, emotionList, buttonList;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        fileNo = 0;
        BGMNo = -1;
        nextXML = string.Empty;
        location = string.Empty;
        soundEffects = string.Empty;
        decision = string.Empty;
        commands = string.Empty;
        xmlRawFile = xmlFiles[fileNo];
        data = xmlRawFile.text;

        textList = new List<string>();
        emotionList = new List<string>();
        buttonList = new List<string>();
    }

    public void switchXML()
    {
        fileNo = int.Parse(nextXML);
        xmlRawFile = xmlFiles[fileNo];
        data = xmlRawFile.text;
        BGMNo = -1;
        nextXML = string.Empty;
        soundEffects = string.Empty;
    }

    public void updateDialogue()
    {
        parseXMLFile(data);
    }

    void parseXMLFile (string xmlData)
    {
        textList.Clear();
        emotionList.Clear();
        buttonList.Clear();
        nextXML = string.Empty;
        location = string.Empty;
        soundEffects = string.Empty;
        decision = string.Empty;
        commands = string.Empty;
        BGMNo = -1;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        string xmlPathPattern = "//document/" + label;
        XmlNodeList myNodeList = xmlDoc.SelectNodes(xmlPathPattern);
        XmlNode firstNode = myNodeList.Item(0);
        XmlNodeList theseNodes = firstNode.ChildNodes;

        for (int i = 0; i < theseNodes.Count; i++)
        {
            //refactored node organization to relevant lists
            XmlNode finalNode = theseNodes[i];
            switch (finalNode.Name)
            {
                case "sprite":
                    emotionList.Add(finalNode.InnerXml);
                    break;

                case "speech":
                    textList.Add(finalNode.InnerXml);
                    break;

                case "button":
                    buttonList.Add(finalNode.InnerXml);
                    break;

                case "trigger":
                    emotionList.Add(finalNode.InnerXml);
                    break;

                case "shift":
                    location = finalNode.InnerXml;
                    break;

                case "BGM":
                    BGMNo = int.Parse(finalNode.InnerXml);
                    break;

                case "SFX":
                    soundEffects = finalNode.InnerXml;
                    break;

                case "next":
                    nextXML = finalNode.InnerXml;
                    break;

                case "choice":
                    decision = finalNode.InnerXml;
                    break;

                case "command":
                    commands = finalNode.InnerXml;
                    break;
            }
        }
    }
}
