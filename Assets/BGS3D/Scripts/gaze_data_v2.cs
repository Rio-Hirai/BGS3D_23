using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class gaze_data_v2 : MonoBehaviour
    {
        private static EyeData eyeData = new EyeData();
        private bool eye_callback_registered = false;
        //private bool has_header_written;
        //private object JsonCsv;
        public GameObject Server;
        private receiver script;

        public StreamWriter csv_results { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            // サーバと接続
            script = Server.GetComponent<receiver>();

            if (!SRanipal_Eye_Framework.Instance.EnableEye)
            {
                enabled = false;
                return;
            }

            //csv_results = new StreamWriter("results/eyedata.csv", false);   //true=追記 false=上書き
            //SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
        }

        // Update is called once per frame
        void Update()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
            ////オブジェクトをJSONに変換
            //string json_txt = JsonUtility.ToJson(eyeData);

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

        private void Release()
        {
            if (eye_callback_registered == true)
            {
                SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
        }

        private void EyeCallback(ref EyeData eye_data)
        {
            eyeData = eye_data;

            //// タイムスタンプ，タスク番号，焦点座標x，焦点座標y，瞳孔径・右，瞳孔径・左，開き具合・右，開き具合・左，頭部角度・x，頭部角度・y，頭部角度・z
            //script.tasklogs3.Add(script.test_time + "," + (script.task_num) + "," + (eye.CombineFocus.point.x) + "," + (CombineFocus.point.y) + "," + (RightPupiltDiameter) + "," + (LeftPupiltDiameter) + "," + (RightOpenness) + "," + (LeftOpenness) + "," + (script.HMDRotation.x) + "," + (script.HMDRotation.y) + "," + (script.HMDRotation.z));

            script.tasklogs3.Add(script.test_time + "," + (script.task_num) + "," + (eyeData.verbose_data.combined.eye_data.gaze_origin_mm.x) + "," + (eyeData.verbose_data.combined.eye_data.gaze_origin_mm.y) + "," + (eyeData.verbose_data.right.pupil_diameter_mm) + "," + (eyeData.verbose_data.left.pupil_diameter_mm) + "," + (eyeData.verbose_data.right.eye_openness) + "," + (eyeData.verbose_data.left.eye_openness));
        }
    }
}
