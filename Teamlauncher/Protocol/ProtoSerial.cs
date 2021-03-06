﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Teamlauncher.Protocol
{
    class ProtoSerial : ProtocolType
    {
        protected string clientExe;

        public override int AllowedParameters
        {
            get
            {
                return ParamHost;
            }
        }

        public ProtoSerial()
        {
            icon = Properties.Resources.serial;
            name = "serial";
            defaultPort = 1;

            Trace.WriteLine("Protocol module " + name + " loaded");

            if (File.Exists(Properties.Settings.Default.Putty))
            {
                clientExe = Properties.Settings.Default.Putty;
            }
            else
            {
                clientExe = "";
            }
        }
        public override void run(int paramSet, string login, string password, string host, int port, string resource)
        {
            string[] serialPortList;
            
            Regex validSerialPortName;

            validSerialPortName = new Regex("^COM[0-9]{1,2}$");
            
            /* detect if any serial port first */
            do
            {
                serialPortList = System.IO.Ports.SerialPort.GetPortNames();
                if (serialPortList.Count() == 0)
                {
                    if (MessageBox.Show("No serial port detected on this computer.", "Serial plugin",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            } while (serialPortList.Count() == 0);

            /* only one serial port detected, force setting on this one */
            if (serialPortList.Count() == 1)
            {
                host = serialPortList[0];
                paramSet |= ProtocolType.ParamHost;
            }

            if (clientExe != "")
            {
                /* no valid provided, try to build from "port" number value */
                if (((paramSet & ProtocolType.ParamHost) == 0) || !(validSerialPortName.IsMatch(host)))
                {
                    if (((paramSet & ProtocolType.ParamPort) > 0) && (port >= 1 && port <= 16))
                    {
                        host = String.Format("COM{0}", port);
                        if (serialPortList.Contains(host))
                        {
                            paramSet |= ProtocolType.ParamHost;
                        }
                        else
                        {
                            paramSet &= ~ProtocolType.ParamHost;
                        }
                    }
                }

                /* we have a host and it is valid serial port name */
                if (((paramSet & ProtocolType.ParamHost) > 0) && (validSerialPortName.IsMatch(host)))
                {
                    if (!serialPortList.Contains(host))
                    {
                        MessageBox.Show(String.Format("Serial port {0} not found!", host), "Serial plugin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        Process.Start(clientExe, String.Format(" -serial {0}", host));
                    }
                }
                else
                {
                    MessageBox.Show("No valid serial port defined!\nPlease specify valid serial port name in Host field.", "Serial plugin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Unable to find installed version of Putty!\nPlease update your .config file.", "Serial plugin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}