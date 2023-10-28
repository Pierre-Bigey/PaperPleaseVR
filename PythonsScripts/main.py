import csv


path = "C:/Users/pierr/Unity Projects/PaperPleaseVR/Assets/Ressources/FirstNamesFemale.csv"
pathCSV = "C:/Users/pierr/Unity Projects/PaperPleaseVR/Assets/Ressources/firstNamesFemale.csv"
csvfile = open(pathCSV,'w',newline='')

with open(path) as file:
    line = file.readline()
    while len(line)>0:
        csvfile.write(line[:-1]+"\n")
        line = file.readline()
    file.close()

csvfile.close()

