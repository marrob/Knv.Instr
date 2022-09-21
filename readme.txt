https://www.codeproject.com/Articles/302873/Remote-Controlling-Instrument-Over-LAN


TCPClient vs Socket in C#
https://stackoverflow.com/questions/685995/tcpclient-vs-socket-in-c-sharp


/*** Debug From TestStand ***/
TestStand 2017
Visual Studio 2019 v 2022

1. Tegyél töréspontot abba a C# metódusba ahol meg szeretnél álni
2. Válaszd a Debug-> Attach to Process menüpontot majd válaszd a SeqEdit.exe-ét a listából
3. Indisd e TestStandbol a progit majd SetpInto-val lépj bele... 
(elofordulahat hogy feldob egy ilyen ablakot hogy: Warning ... Could not find DIA CLSID for this version of Visual Studio)


NI-VISA .NET Library
https://www.ni.com/hu-hu/support/documentation/supplemental/15/national-instruments-visa--net-library.html

.NET Resources for NI Hardware and Software
https://www.ni.com/hu-hu/support/documentation/supplemental/13/national-instruments--net-support.html


Tipusok
1. nativ ethernet tcp-ip: 192.168.100.8 port
 
2. ethernet VXI-11 :TCPIP0::192.168.100.8::inst0::INSTR


/* 
 * --- NI VISA ---
 * NationalInstruments.Visa
 * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll
 * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll

 *Ivi.Visa
 * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v2.0.50727\VISA.NET Shared Components 5.8.0\Ivi.Visa.dll
 *C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll
*/

--- KESYIGHT VISA ---
Ivi.Visa.Interop



