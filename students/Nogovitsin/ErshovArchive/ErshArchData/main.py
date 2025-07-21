import re


with  open("docs.csv", "r", encoding="UTF-8") as f:
    text = f.read()
print([text[:100000]])
print(re.search('\N', text))




