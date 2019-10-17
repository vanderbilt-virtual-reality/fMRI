import re
import csv

with open("../data/Parcellation_96.txt", 'r') as file:
    data = file.read()

list = []
lines = re.split(r'\n+', data)
for line in lines:
    fields = re.split(r'\t+', line)
    row = []
    for field in fields:
        if (len(field) > 0 and (field.isnumeric() or field[0] == '-')):
            row.append(float(field))
    if (len(row) == 4):
        list.append(row[1:])

with open("../data/coordinate_data.csv", "w", newline="") as f:
    writer = csv.writer(f)
    writer.writerows(list)


