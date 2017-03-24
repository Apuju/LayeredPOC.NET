using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuspiciousObjectExporter.Models;

namespace SuspiciousObjectExporter
{
    public class Program
    {
        static Int32 m_startID = 0;
        static Int32 m_endID = 0;
        static Int32 m_lastID = 0;
        static Boolean m_enableFreezeConsoleOrNot = true;
        static Boolean m_debugModeFlag = false;

        static Int32 Main(string[] args)
        {
            String m_dateTimePattern = "yyyy/MM/dd HH:mm:ss";
            Logger m_logger = new Logger();
            StringBuilder m_log = new StringBuilder();

            #region Access Arguments
            Boolean checkInputArgumentFlag = true;
            String checkInputArgumentExceptionMessage = String.Empty;
            String argumentVariableCache = String.Empty;
            if (args.Length < 1)
            {
                checkInputArgumentFlag = false;
                checkInputArgumentExceptionMessage = LocalResource.SuspiciousObjectExporterConsole.illeaglInfoString05;
            }
            else
            {
                try
                {
                    foreach (String argument in args)
                    {
                        if (String.IsNullOrEmpty(argumentVariableCache) && argument.Length != 2)
                        {
                            checkInputArgumentFlag = false;
                            checkInputArgumentExceptionMessage = String.Format("{0} {1} {2}", LocalResource.SuspiciousObjectExporterConsole.illeaglInfoString04, LocalResource.SuspiciousObjectExporterConsole.ArrowIcon01, argument);
                            break;
                        }
                        if (String.IsNullOrEmpty(argumentVariableCache))
                        {
                            argumentVariableCache = argument;
                        }
                        else
                        {
                            if (!CheckArgument(argumentVariableCache, argument))
                            {
                                checkInputArgumentFlag = false;
                                checkInputArgumentExceptionMessage = String.Format("{0} {1} {2}:{3}", LocalResource.SuspiciousObjectExporterConsole.illeaglInfoString04, LocalResource.SuspiciousObjectExporterConsole.ArrowIcon01, argumentVariableCache, argument);
                                argumentVariableCache = String.Empty;
                                break;
                            }
                            else
                            {
                                argumentVariableCache = String.Empty;
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(argumentVariableCache))
                    {
                        if (!CheckArgument(argumentVariableCache, null))
                        {
                            checkInputArgumentFlag = false;
                            checkInputArgumentExceptionMessage = String.Format("{0} {1} {2}", LocalResource.SuspiciousObjectExporterConsole.illeaglInfoString04, LocalResource.SuspiciousObjectExporterConsole.ArrowIcon01, argumentVariableCache);
                        }
                        argumentVariableCache = String.Empty;
                    }
                }
                catch (Exception ex)
                {
                    ShowException(ex);
                }
                #region Check Start ID and End ID
                if (m_startID > m_endID || (m_startID == m_endID && m_startID != 0))
                {
                    checkInputArgumentFlag = false;
                    checkInputArgumentExceptionMessage = String.Format("{0} {1} {2}-{3}", LocalResource.SuspiciousObjectExporterConsole.illeaglInfoString06, LocalResource.SuspiciousObjectExporterConsole.ArrowIcon01, m_startID, m_endID);
                }
                #endregion
            }
            if (!checkInputArgumentFlag)
            {
                ShowOnlineHelp();
                ShowWarning(checkInputArgumentExceptionMessage);
                ShowInputHelp();
            }
            #endregion

            #region show content header on the console
            String welcomeToExporterString = String.Format("{0} {1} {2}",
                LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01,
                LocalResource.SuspiciousObjectExporterConsole.ApplicationWelocomeString,
                LocalResource.SuspiciousObjectExporterConsole.ArrowIcon01);
            ShowLine(welcomeToExporterString);
            #endregion

            #region log content header on the log file
            m_log.AppendLine(LocalResource.SuspiciousObjectExporterConsole.DivideIcon01);
            m_log.Append(String.Format("{0} {1}",
                welcomeToExporterString,
                DateTime.Now.ToString(m_dateTimePattern)));
            m_logger.LogIt(m_log.ToString());
            m_log = new StringBuilder();
            #endregion

            #region Export suspicouis object list
            using (Exporter exporter = new Exporter())
            {
                try
                {
                    exporter.DebugMode = m_debugModeFlag;
                    String exportFile = exporter.WriteSuspiciosObjectList(m_startID, m_endID);

                    #region show export information on the console
                    String exportInfoString = String.Format("{0} {1} {2} {3}", 
                        LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01, 
                        LocalResource.SuspiciousObjectExporterConsole.ImportSuccessfully,
                        LocalResource.SuspiciousObjectExporterConsole.ArrowIcon01,
                        exportFile);
                    ShowLine(exportInfoString);
                    #endregion

                    #region log export information on the log file
                    m_log.AppendLine(String.Format("{0} {1} {2} {3} {4}", 
                        LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01, 
                        LocalResource.SuspiciousObjectExporterConsole.SORangeInfoString01, 
                        m_startID, 
                        LocalResource.SuspiciousObjectExporterConsole.SORangeInfoString02, 
                        m_lastID));
                     m_log.Append(exportInfoString);
                     m_logger.LogIt(m_log.ToString());
                     m_log = new StringBuilder();
                    #endregion

                }
                catch (Exception ex)
                {
                    ShowException(ex);
                }
            }
            #endregion

            #region show content footer on the console
            String importFinishString = String.Format("{0} {1} {2}",
                LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01,
                LocalResource.SuspiciousObjectExporterConsole.ImportFinishString,
                LocalResource.SuspiciousObjectExporterConsole.ArrowIcon02);
            ShowLine(importFinishString);
            #endregion

            #region log content footer on the log file
            m_log.AppendLine(String.Format("{0} {1}",
                importFinishString,
                DateTime.Now.ToString(m_dateTimePattern)));
            m_logger.LogIt(m_log.ToString());
            m_log = new StringBuilder();
            #endregion

            #region Freeze the console after executed
            if (m_enableFreezeConsoleOrNot)
            {
                FreezeConsole();
            }
            #endregion

            return m_lastID;
        }

        #region Fetch Last SeqID
        internal static void FetchLastID(Int32 id)
        {
            if (id > m_lastID)
            {
                m_lastID = id;
            }
        }
        #endregion

        #region Check the arguments is correct or not
        internal static Boolean CheckArgument(String key, String value)
        {
            Boolean pass = true;
            if (key.Equals("/s"))
            {
                if (!IsNumeric(value))
                {
                    pass = false;
                }
                else
                {
                    m_startID = Convert.ToInt32(value);
                }
            }
            else if (key.Equals("/e"))
            {
                if (!IsNumeric(value))
                {
                    pass = false;
                }
                else
                {
                    m_endID = Convert.ToInt32(value);
                }
            }
            else if (key.Equals("/f"))
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (value.ToLower().Equals("y"))
                    {
                        m_enableFreezeConsoleOrNot = true;
                    }
                    else if (value.ToLower().Equals("n"))
                    {
                        m_enableFreezeConsoleOrNot = false;
                    }
                    else
                    {
                        pass = false;
                    }
                }
                else
                {
                    pass = false;
                }
            }
            else if (key.Equals("/d"))
            {
                if (!String.IsNullOrEmpty(value))
                {
                    pass = false;
                }
                else
                {
                    m_debugModeFlag = true;
                }
            }
            else
            {
                pass = false;
            }
            return pass;
        }
        #endregion

        #region Base Access for Console
        internal static void ShowLine(String content)
        {
            Console.WriteLine(content);
        }

        internal static void FreezeConsole()
        {
            String exitHelpString = String.Format("{0} {1}",
                LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01,
                LocalResource.SuspiciousObjectExporterConsole.ExitApplicationString);
            ShowLine(exitHelpString);
            Console.Read();
        }
        #endregion

        #region Check numeric or not
        internal static bool IsNumeric(String value)
        {
            try
            {
                Int32 number = Convert.ToInt32(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Check boolean or not
        internal static bool IsBoolean(String value)
        {
            try
            {
                Boolean booleanValue = Convert.ToBoolean(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region [Module] Console Message Workflow

        #region Show some information for second plus input
        internal static void ShowInputHelp()
        {
            Boolean inputLoop = true;

            #region fetch the start ID from the console by the user
            while (inputLoop)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                ShowLine(LocalResource.SuspiciousObjectExporterConsole.InputInfoString01);
                Console.BackgroundColor = ConsoleColor.Black;
                String inputStartID = Console.ReadLine();
                if (IsNumeric(inputStartID))
                {
                    m_startID = Convert.ToInt32(inputStartID);
                    inputLoop = false;
                }
                else
                {
                    ShowWarning(LocalResource.SuspiciousObjectExporterConsole.NumericWarningString);
                }
            }
            #endregion

            inputLoop = true;

            #region fetch the end ID from the console by the user
            while (inputLoop)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                ShowLine(LocalResource.SuspiciousObjectExporterConsole.InputInfoString02);
                Console.BackgroundColor = ConsoleColor.Black;

                String inputEndID = Console.ReadLine();
                if (IsNumeric(inputEndID))
                {
                    m_endID = Convert.ToInt32(inputEndID);
                    inputLoop = false;
                }
                else
                {
                    ShowWarning(LocalResource.SuspiciousObjectExporterConsole.NumericWarningString);
                }
            }
            #endregion
        }
        #endregion

        #region Show the warning information
        internal static void ShowWarning(String content)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            ShowLine(content);
            Console.BackgroundColor = ConsoleColor.Black;
        }
        #endregion

        #region Show Online Help
        internal static void ShowOnlineHelp()
        {
            StringBuilder onlineHelp = new StringBuilder();
            /*
             * USAGE : 
             *         SuspiciousObjectExporter.exe [/s [Start ID]] [/e [End ID]] [/h [y|n]] [/d]
             * where
             *         Start ID        The first ID for querying the suspicious object list
             *         End ID         The latest ID for querying the suspicious object list
             *         
             *         Options :
             *             /s              Specifies the first ID of the suspicious object to be queried
             *             /e              Specifies the latest ID of the suspicious object to be queried
             *             /h              Enables the action which the console freeze after executed or not for powershell or windows scheduler
             *             /d              Enable dubug mode
             * 
             * Example :
             *         > SuspiciousObjectExporter.exe /s 0 /e 0               ... Export all of the suspicious object and freeze the console windows after excuted
             *         > SuspiciousObjectExporter.exe /s 1 /e 2               ... Export the suspicious object from ID NO.1 to ID NO.2
             *         > SuspiciousObjectExporter.exe /s 0 /e 0 /h y         ... Export all of the suspicious object and freeze the console windows after excuted
             *         > SuspiciousObjectExporter.exe /s 0 /e 0 /h n         ... Export all of the suspicious object and don't freeze the console windows after excuted
             *         > SuspiciousObjectExporter.exe /d                        ... Enable dubug mode
             */
            ShowLine(LocalResource.SuspiciousObjectExporterConsole.HelpInfoString);
        }
        #endregion

        #region Show excption inoformation
        internal static void ShowException(Exception ex)
        {
            String exceptionInformation = String.Format("{0} {1} {2}",
                            LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01,
                            LocalResource.SuspiciousObjectExporterConsole.ExceptionTitleString,
                            ex.Message);

            #region log Exception information on the log file
            Logger m_logger = new Logger();
            try
            {
                m_logger.LogIt(exceptionInformation);
            }
            catch (Exception unexpexctedException)
            {
                exceptionInformation = String.Format("{0} {1} {2}",
                    LocalResource.SuspiciousObjectExporterConsole.DirectorIcon01,
                    LocalResource.SuspiciousObjectExporterConsole.ExceptionTitleString,
                    unexpexctedException.Message);
            }
            #endregion
            ShowLine(exceptionInformation);
        }
        #endregion

        #endregion
    }
}
