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
1. Nativ Ethernet TCP/IP - RAW SOCKET - SCPI-RAW (TCP/UDP)(IANA official)
   - Ehhez csak egy IP és protszám kell ami az 5025-ös
   - VISA telepítése nékül is megy
   - TCPClient vs Socket in C#: https://stackoverflow.com/questions/685995/tcpclient-vs-socket-in-c-sharp
   - https://www.codeproject.com/Articles/302873/Remote-Controlling-Instrument-Over-LAN
   
2. VXI-11 Ethernet
  - pl: TCPIP0::192.168.100.8::inst0::INSTR
  - A TCP/IP fölött még egy réteg van a VXI-11 (https://www.lxistandard.org/about/vxi-11-and-lxi.aspx)
  - Kell Hozzáa NI-MAX és a VISA 
  - Ezt lehet a "Ivi.Visa.Interop.dll" vagy a "NationalInstruments.Visa.dll"
  - A soroportot biztos lehet használni VISA telepítése nélkül kizárolag a NationalInstruments.Visa.dll-eket használva
3. USB
   - VCP
   - USBTMC Driver - USB Test & Measurement Class

/* 
 * --- NI VISA ---
 * NationalInstruments.Visa
 * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll
 * C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\NI VISA.NET 19.0\NationalInstruments.Visa.dll

 *Ivi.Visa
 * C:\Program Files(x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v2.0.50727\VISA.NET Shared Components 5.8.0\Ivi.Visa.dll
 *C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.11.0\Ivi.Visa.dll
*/

--- Álatalános VISA ---
Ivi.Visa.Interop.dll
ezt használja a Keisight is.



