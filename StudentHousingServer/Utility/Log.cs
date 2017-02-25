using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utility
{
    ///
    /// \class Logging
    /// \brief This class is called upon in order to audit (or track) the usage , status and outcomes of many of the other class libraries methods
    /// \details logging class will use FileIO class to access log files \n
    ///          The following events should be logged: \n
    ///          - in any case, once a record is read and parsed from the incoming file, the Logging class is used to record this event – this logging event would indicate if the record just parsed generated a valid employee-type or not \n
    ///          - include a summary event when this is done indicating the total number of records read, total number valid and total number invalid \n
    ///          - include a summary event when this is done indicating the total number of records written, total number valid and total number invalid \n
    ///          - any fileIO related errors \n
    ///          - Anytime an attribute is attempting to be set, but is invald – log the attribute name and invalid attribute value 	\n
    ///          - When the Validate() method is called – log the employee’s name (lastName, firstName) and socialInsuranceNumber values as well as whether the object is valid or not	\n
    ///          - When the Details() method is called – log all employee details in the same format as that being displayed on the screen into the log file	\n
    ///
    public class Logging
    {

        ///
        /// \brief This function logs important event
        /// \details <b>Details</b> 
        /// \param className - <b>string</b> - the class name of the caller method
        /// \param methodName - <b>string</b> - the name of the caller method
        /// \param sEvent - <b>string</b> - the event descripsion
        /// \param result - <b>bool</b> - the return value of the caller method
        /// \return <b>bool </b> - true if log was saved to file successfully, false if log was saved to file unsuccessfully
        /// 
        public static void Log(string className, string methodName, string sEvent, bool result)
        {
            string log = LogBuilder(className, methodName, sEvent, result);
            WriteToFile(log);

        }


        ///
        /// \brief This function buid the log string
        /// \details <b>Details</b> 
        /// \param className - <b>string</b> - the class name of the caller method
        /// \param methodName - <b>string</b> - the name of the caller method
        /// \param sEvent - <b>string</b> - the event descripsion
        /// \param result - <b>bool</b> - the return value of the caller method
        /// \return log <b>string </b> - the log string that was built using the passed in parameters
        /// 
        private static string LogBuilder(string className, string methodName, string sEvent, bool result)
        {
            string sResult = "";
            // get date & time
            string dateTime = DateTime.Now.ToString();

            if (result == true)
            {
                sResult = "SUCCESS";
            }
            else
            {
                sResult = "FAIL";
            }
            string log = dateTime + " [" + className + "." + methodName + "] " + sEvent + " - " + sResult;

            return log;

        }


        ///
        /// \brief This function writes log to log files
        /// \details <b>Details</b> 
        /// \param log - <b>string</b> - the log string that was built using the logbuilder()
        /// \return true - <b>bool </b> -  if log was saved to file successfully, 
        /// \return false - <b>bool </b> - if log was saved to file unsuccessfully
        /// 
        private static bool WriteToFile(string log)
        {
            // get date, and appropriate log file name e.g.ems.YYYY-MM-DD.log
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = "ems." + date + ".log";

            try
            {
                var fileIO = new FileIO();

                fileIO.CreateFile(fileName, "log", "*.log");
                StreamWriter sw = fileIO.GetStreamWriter(fileName, "log", "*.log");
                sw.WriteLine(log);
                sw.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }


        }

    }
}
