using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ivi.Driver;
using Ivi.DCPwr;

/////////////////////////////////////////////////////////////////////////////
// Name:        IviDcPwr_Simple_Example
//
// Purpose:     IviDcPwr sample program to demonstrate simple setting of Voltage, Current limit on two channels + measurement
//              using an R&S HMP804x/NGE10x power supplies
//
//              The Example uses only the interchangeable Ivi.Driver and Ivi.DcPwr Interface.
//              Therefore, notice please, there is no reference to the Rohde & Scharz RsHmc804x instrument driver.
//              The link to pointing to the RsHmc804x is the following:
//              - Logical Name (e.g. "myDCPwr") here used in the method IviDCPwr.Create() which points to
//              - Driver Session (e.g. "myDCPwrSession") which has a property SoftwareModule "RohdeSchwarz.RsHmc804x.Fx40"
//              (see the settings in Ivi Config Store configuration below in Prerequisities)
//              Therefore, when using Ivi-class drivers, you always need to use configured logical name, otherwise
//              the information about which instrument driver to use with which Resource name would be missing.
//
// Author:      
// Created:     04-April-2018
// Modified by: Miloslav Macko, 1DC3
// Copyright:   (c) Rohde & Schwarz, Munich
/////////////////////////////////////////////////////////////////////////////

//--------------------------------------------------------------------------
// Prerequisites
//
//   This sample program needs IVI Shared Components and IVI.NET Shared Components being installed.
//   If not yet done, visit
//     http://www.ivifoundation.org/shared_components/Default.aspx
//
//   Download and install
//     - IVI Shared Components 2.6.0
//     - IVI.NET Shared Components 1.4.0
//     - Rohde & Schwarz RsHmc804x IVI.NET driver 1.5.1
//
//   32-bit: IviSharedComponents_2.6.0.exe
//           IviNetSharedComponents32_Fx20_1.4.0.exe
//           RsHmc804x-ivi.net-x86-1_5_1.msi
//
//   64-bit: IviSharedComponents64_2.4.2.exe
//           IviNetSharedComponents64_Fx20_1.4.0.exe
//           RsHmc804x-ivi.net-x64-1_5_1.msi
//
//   You always need to install BOTH packages for your processor architecture

//   Configure your instrument in the IviConfigStore. The description below is for the NI MAX:
//   - System -> IVI Drivers -> Driver Sessions -> CreateNew "myDCPwrSession"
//     - In the right tab "Software" (selectable at the bottom) choose SoftwareModule "RohdeSchwarz.RsHmc804x.Fx40"
//     - In the right tab "Hardware" (selectable at the bottom) select/add one instrument in the HardwareAssets table e.g.:
//       Name = "NGE100", ResourceDescriptor = "USB::0x0AAD::0x0197::100433::INSTR"
//   - System -> IVI Drivers -> Logical Names -> CreateNew "myDCPwr" - this is the name you refer to your instrument in the Create() method
//     - In right tab "General" choose Driver Session "myDCPwrSession"
//   - Save the IVI Configuration
//--------------------------------------------------------------------------

namespace IviDcPwr_SimpleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            bool id_query = true;
            bool reset_device = true;
            IIviDCPwr driver = null;
            

            Console.Write("Initializing the instrument ... ");
            try
            {
                // Open the driver session, but only use the device-interchangeable IIviDcPwr interface
                driver = IviDCPwr.Create("myDCPwr", id_query, reset_device, "Simulate=False");
                Console.WriteLine("finished");
            }
            catch (Exception ex)
            {
                if (ex is Ivi.Driver.DriverClassCreationException
                    || ex is Ivi.Driver.SessionNotFoundException
                    || ex is Ivi.Driver.IOException)
                {
                    Console.WriteLine("\nException '{0}' occured.", ex.GetType());
                    Console.WriteLine("Check your instrument and your Ivi Config Store Configuration.\nDetails:");
                    Console.WriteLine(ex.Message);
                    Console.Write("\nPress any key to finish ... ");
                    Console.ReadKey();
                    return;
                }
                throw; // rethrow the exception if it is of other kind
            }

            try
            {
                // Settings for Channel 1
                Console.Write("Configuring Channel 1 ... ");
                driver.Outputs["1"].VoltageLevel = 1.12;
                driver.Outputs["1"].OvpLimit = 2.5;
                driver.Outputs["1"].OvpEnabled = true;
                driver.Outputs["1"].CurrentLimit = 0.123;
                driver.Outputs["1"].Enabled = true;
                Console.WriteLine("finished");
                var measVoltageCH1 = driver.Outputs["1"].Measure(MeasurementType.Voltage);
                Console.WriteLine("Measured Voltage on Channel 1: {0:F3} V", measVoltageCH1);

                // Same settings for Channel 2
                Console.Write("Configuring Channel 2 ... ");
                driver.Outputs["2"].VoltageLevel = 2.34;
                driver.Outputs["2"].OvpLimit = 3.6;
                driver.Outputs["2"].OvpEnabled = false;
                driver.Outputs["2"].Enabled = true;
                Console.WriteLine("finished");
                var measVoltageCH2 = driver.Outputs["2"].Measure(MeasurementType.Voltage);
                Console.WriteLine("Measured Voltage on Channel 2: {0:F3} V", measVoltageCH2);
            }
            //driver IO error
            catch (Ivi.Driver.IOException e)
            {
                Console.WriteLine("Driver error occured:");
                Console.WriteLine(e.Message);
            }

            //driver function is not supported
            catch (Ivi.Driver.OperationNotSupportedException e)
            {
                Console.WriteLine("Instrument doesn't support the function:");
                Console.WriteLine(e.Message);
            }

            //when a task takes longer than the defined timeout
            catch (Ivi.Driver.MaxTimeExceededException e)
            {
                Console.WriteLine("Operation took longer than the maximum defined time:");
                Console.WriteLine(e.Message);
            }

            //if the instrument returns an error in the error queue
            catch (Ivi.Driver.InstrumentStatusException e)
            {
                Console.WriteLine("Instrument system error occured:");
                Console.WriteLine(e.Message);
            }

            //if the instrument returns an error in the error queue
            catch (Ivi.Driver.SelectorNameException e)
            {
                Console.WriteLine("Invalid selector name used:");
                Console.WriteLine(e.Message);
            }

            finally
            {
                Console.Out.Write("\n\nPress any key to finish ... ");
                Console.ReadKey();
            }
        }
    }
}