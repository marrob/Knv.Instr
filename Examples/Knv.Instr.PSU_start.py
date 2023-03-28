# -*- coding: utf-8 -*-
"""
Created on Fri Feb 17 08:56:52 2023

@author: marrob
"""

import os


#*** Test Stand ****
# SeqEdit.exe /env "MyEnvironment.tsenv"
#
seqEditPath = r"C:\Program Files (x86)\National Instruments\TestStand 2019\Bin\SeqEdit.exe"
sequencePath = fr"{os.getcwd()}\Knv.Instr.PSU.seq"
tsenvPath = fr"{os.getcwd()}\Instr.tsenv"
os.startfile(seqEditPath, arguments = f"{sequencePath} /env {tsenvPath}")
