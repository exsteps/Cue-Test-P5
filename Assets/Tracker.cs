using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;


/*! \An example tracking script that logs the world position of the object it is attached to.
 *
 *  Set the logs per second and where to output the saved positions.  These can be read by the Heatmap class and turned into a Heatmap.
 */
public class Tracker : MonoBehaviour {

    private int version = 2;


    public Boolean RawFormat = true;                           // if true => one file only will be generated; (new output); if false a json file ans a vtt file will generated (old output)

    public float LogsPerSecond = 1f;                     // Default to one log per second
    public string fileName = "TrackPoints";
    public Boolean AddDate2File = true;                   // add the current date and time to the log file nam
    public string Tag = "";                     // an user defined string as tag, if "" => take the m_startTime string as Tag
    public string TagPrefix = "H_";
    public string StartTime;                               // out only: the start time as formated string
    public string SaveTime;                                // out only: the start time as formated string
    public double CurrentSeekPosition;                     // out only: the current seek position of the movie
    public int NumberOfPoints;                          // out only: the current number of logged points
    public int PointsPerSave = 1000;                    // save after every <PointsPerSave> tracking points, defaults to 1000

    private string HeatmapFile;          // default log file name
    private string m_heatmapPath;
    private float m_logDelay;
    private float m_timer;
    private Boolean m_trackingStarted;
    private DateTime m_startTime;
    private string m_movieName;
    private StringBuilder m_trackString;
    private int m_partcnt;
    public string probandID;

    public void Start() {
        Initialize();
    }

    public void OnEnable() {
        Initialize();
    }

    private void Initialize() {
        Debug.Log("UNITY360 >>>>>>>>>>>>>> Start");

        m_heatmapPath = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + "Heatmaps";
        if (!Directory.Exists(m_heatmapPath)) {
            Directory.CreateDirectory(m_heatmapPath);
        }

        m_logDelay = 1f / LogsPerSecond;
        m_timer = 0f;
      
        m_trackingStarted = false;
        m_partcnt = 0;

        // remove file extension
        GameObject movieCanvas = GameObject.FindGameObjectWithTag("VideoPlayer");
        VideoPlayer m_script = movieCanvas.GetComponent<VideoPlayer>();
        m_movieName = System.IO.Path.GetFileName(m_script.clip.name); //remove the path, use the file name only
        Debug.Log("UNITY360  ... Movie File Name = " + m_movieName);
        probandID = "_ID_NOT_SET_";


        HeatmapFile = probandID + "_" + fileName + "_" + System.IO.Path.GetFileNameWithoutExtension(m_movieName);  // heatmap file name = movie name without extension

        if (RawFormat == true) {
            Debug.Log("UNITY360  ... Using Raw Format");
            m_trackString = new StringBuilder("version: " + version + "\n");
            m_trackString.Append("movie: " + m_movieName + "\n");
            m_trackString.Append("logDelay: " + (int)(m_logDelay * 1000) + "\n");
        }
    }

    public void Update() {  
        m_timer += Time.deltaTime;

        GameObject movieCanvas = GameObject.FindGameObjectWithTag("VideoPlayer");
        VideoPlayer m_script = movieCanvas.GetComponent<VideoPlayer>();
        if (m_trackingStarted == false && m_script.isPlaying) {
            // this is called only one time at the begin of movie
            Debug.Log("UNITY360 >>>>>>>>>>>>>> Update(): PLaying State = PLAYING");
            m_trackingStarted = true;
            m_startTime = DateTime.Now;
            m_startTime = DateTime.Now;
            StartTime = m_startTime.ToString("yyyy-MM-dd_HH:mm:ss");
            CurrentSeekPosition = m_script.time;
            if (AddDate2File == true) {
                //HeatmapFile = HeatmapFile + "_" + m_startTime.ToString("yyyyMMddHHmmss");
                HeatmapFile = HeatmapFile + "_" + m_startTime.ToString("dd-MM-yyyy--HH-mm-ss");
            }
            if (Tag == "") {
                Tag = m_startTime.ToString("yyyyMMddHHmmss");
            }
            Tag = TagPrefix + Tag;

            //if (RawFormat == true) {
            m_trackString.Append("tag: " + Tag + "\n");
            m_trackString.Append("time: " + StartTime + "\n\n");
            m_trackString.Append("# seekPos  x  y\n");
            //}
            m_timer = 0f;
            LogRotation(gameObject.transform.rotation, CurrentSeekPosition);
        }
        if (m_trackingStarted == true && m_script.time >= m_script.clip.length) {
            // this is called only one time at the end of movie
            Debug.Log("UNITY360 >>>>>>>>>>>>>> Update(): Playing State = END");
            m_trackingStarted = false;
        }

        if (m_trackingStarted == true && m_timer > m_logDelay) {
            m_timer = 0f;
            CurrentSeekPosition = m_script.time;
            //Debug.Log("UNITY360 >>>>>>>>>>>>>> Update(): seekPos = " + CurrentSeekPosition);
            LogRotation(gameObject.transform.rotation, CurrentSeekPosition);
        }
    }

    public void OnDisable() {
        Debug.Log("UNITY360 >>>>>>>>>>>>>> OnDisable()");
        // final save the logs and write the output file(s)
        // some logs are already written in LogRotation() (every <PointsPerSave> calls by the Update function
        // otherwise we have a string, witch could be to big for the android
        SaveTime = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        FinalSaveLog(m_heatmapPath + System.IO.Path.DirectorySeparatorChar + HeatmapFile);
    }

    // -------------------------------------------------------------------------------

    private void LogRotation(Quaternion rotation, double seekPosition) {

        NumberOfPoints++;

        LogTrackPoint2RawString(rotation, seekPosition);
 
    }


    private string Log2JsonString() {

        string str = "{\n";
        str += "    \"startTime\": \"" + StartTime + "\",\n";
        str += "    \"saveTime\": \"" + SaveTime + "\",\n";
        str += "    \"movieName\": \"" + m_movieName + "\",\n";
        str += "    \"vttName\": \"" + HeatmapFile + ".vtt\",\n";
        str += "    \"logDelay\": " + (int)(m_logDelay * 1000) + ",\n";
        str += "    \"numberOfPoints\": " + NumberOfPoints + ",\n";
        str += "    \"tag\": \"" + Tag + "\",\n";
        str += "    \"numberOfTags\": " + 1 + "\n";
        str += "}";

        return str;
    }


    private void LogTrackPoint2RawString(Quaternion rotation, double seekPosition) {

        //Debug.Log("UNITY360 LogTrackPoint2RawString(): i = " + NumberOfPoints + ", Pos = " + seekPosition);

        string timePos = String.Format("{0:0.000}", seekPosition);
        m_trackString.Append(timePos + " " + rotation.eulerAngles.x + " " + rotation.eulerAngles.y + "\n");

        if (NumberOfPoints % PointsPerSave == 0) {
            SaveRotationLog();
        }

    }

    private void SaveRotationLog() {

        Debug.Log("UNITY360 SaveRotationLog(): saving data, Part Count = " + m_partcnt);

        string file = m_heatmapPath + System.IO.Path.DirectorySeparatorChar + HeatmapFile + ".raw";
        m_partcnt++;

        System.IO.File.AppendAllText(file, m_trackString.ToString());
        m_trackString = m_trackString.Remove(0, m_trackString.Length);
    }



    private void FinalSaveLog(string path) {
        Debug.Log("UNITY360 >>>>>>>>>>>>>> FinalSaveLog(): Path = " + path + ", L = " + NumberOfPoints);

        // save vtt/raw file, if we still have to save some points
        if (NumberOfPoints > m_partcnt * PointsPerSave) {
            Debug.Log("UNITY360 ... FinalSaveLog(): saving vtt/raw: remaining " + (NumberOfPoints - m_partcnt * PointsPerSave) + " points to save");
            SaveRotationLog();
        }

    }

}
