# -*- coding: utf-8 -*-
"""
@author: marrob
"""

import os


#*** Test Stand ****
# SeqEdit.exe /env "MyEnvironment.tsenv"
#
seqEditPath = r"C:\Program Files (x86)\National Instruments\TestStand 2019\Bin\SeqEdit.exe"
sequencePath = fr"{os.getcwd()}\Knv.Instr.DMM.seq"
tsenvPath = fr"{os.getcwd()}\Instr.tsenv"
os.startfile(seqEditPath, arguments = f'"{sequencePath}" /env "{tsenvPath}"')

print(f"Working directory:{os.getcwd()}")
print(f"Starting Sequence:'{sequencePath}'")
print(f'TestStand argument:{sequencePath} /env "{tsenvPath}"')

input()