using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class gaze_data_v3 : MonoBehaviour
    {
        private static EyeData eyeData = new EyeData();
        private bool eye_callback_registered = false;
        private bool has_header_written;
        private object JsonCsv;
        public GameObject Server;
        private receiver script;

        public StreamWriter csv_results { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            // サーバと接続
            script = Server.GetComponent<receiver>();

            csv_results = new StreamWriter("results/eyedata.csv", false);   //true=追記 false=上書き
            SRanipal_Eye.WrapperRegisterEyeDataCallback(
                Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
        }

        // Update is called once per frame
        void Update()
        {
            ////オブジェクトをJSONに変換
            //string json_txt = JsonUtility.ToJson(eyeData.verbose_data);

            ////JSONからCSVのヘッダに変換
            //if (!has_header_written)
            //{
            //    string csv_header = JsonCsv.JsonToCsvHeader(json_txt);
            //    has_header_written = true;
            //    csv_results.Write(csv_header);
            //}

            ////JSONからCSVのデータ1行に変換
            //string csv_row = JsonCsv.JsonToCsvRow(json_txt);
            //csv_results.Write(csv_row);
        }

        private void EyeCallback(ref EyeData eye_data)
        {
            eyeData = eye_data;


        }
    }
}
