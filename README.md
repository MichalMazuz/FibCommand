# File Bundler CLI

File Bundler CLI הוא כלי שורת פקודה שמטרתו לאגד קבצי קוד ממספר שפות לתוך קובץ יחיד, תוך מתן אפשרויות לסינון, מיון, והוספת מטא-מידע לקובץ המאוגד.

## תכונות עיקריות

**איסוף קבצי קוד**: איסוף קבצים מהתיקייה הנוכחית וכל תת-התיקיות בהתאם לסיומות שנבחרו (כגון .cs, .java, .py ועוד).
**אפשרות למיון**: מיון הקבצים לפי שם הקובץ או לפי סוג הקובץ (סיומת).
**הסרת שורות ריקות**: אפשרות להסיר שורות ריקות מהקבצים המאוגדים.
**הוספת הערות מקור**: הוספת הערות בקובץ המאוגד שמציינות את נתיב הקובץ המקורי.
**הוספת שם יוצר**: אפשרות להוסיף הערת כותרת עם שם היוצר בקובץ המאוגד.
**יצירת קובץ תגובה (Response File)**: אפשרות ליצירת קובץ תגובה שמכיל פקודה מוכנה מראש לצורך הרצה עתידית.

## מבנה הפרויקט

הפרויקט מכיל שני פקודות עיקריות:

### 1. bundle

הפקודה **bundle** משמשת לאיסוף ואיגוד של קבצי קוד בהתאם לאפשרויות שהמשתמש בוחר.

**תחביר הפקודה:**bash
fib bundle --output <output_file> --language <languages> [options]
**פרמטרים:**
--output, -o (חובה): קובץ היעד לאיחוד (לדוגמה, bundleFile.txt).
--language, -l (חובה): רשימה של שפות תכנות (סיומות) לכלול, כגון cs, java, py. ניתן להשתמש ב-all לכל הסיומות הנתמכות.
--sort, -s (ברירת מחדל: name): מיון לפי שם הקובץ (name) או לפי סוג הקובץ (type).
--remove-empty-lines, -r: הסרת שורות ריקות מהקבצים.
--note, -n: הוספת נתיב המקור של כל קובץ כהערה בקובץ המאוגד.
--author, -a: שם היוצר שיופיע ככותרת בראש הקובץ.

**דוגמה לשימוש:**bash
fib bundle --output bundleFile.txt --language cs,java --sort type --remove-empty-lines --note --author "John Doe"
**הסבר:**
איסוף קבצים עם הסיומות cs ו-java.
מיון לפי סוג הקובץ.
הסרת שורות ריקות.
הוספת הערות מקור וקרדיט ליוצר בשם John Doe.
שמירת התוכן המאוגד בקובץ bundleFile.txt.

---

### 2. create-rsp

הפקודה **create-rsp** יוצרת קובץ תגובה (Response File) שמכיל פקודה מוכנה מראש להרצה מאוחרת.

**תחביר הפקודה:**bash
fib create-rsp
**תהליך ההרצה:**
1. המשתמש מזין את השדות הנדרשים (קובץ הפלט, שפות, אפשרויות נוספות).
2. נוצר קובץ bundle-command.rsp עם הפקודה המלאה.
3. ניתן להריץ את הפקודה באמצעות:
   bash
   fib @bundle-command.rsp
   
---

## קבצים וסיומות נתמכות

הכלי תומך בסיומות הבאות:
cs (C#)
java (Java)
py (Python)
txt
js (JavaScript)
html
css

**תיקיות שיוחרגו**: bin, debug, obj, .git.

---

## טיפול בשגיאות

הכלי מטפל בשגיאות אפשריות כמו:
**תיקייה לא קיימת**: במקרה של שגיאה בנתיב הפלט.
**שגיאות גישה**: אם אין הרשאות לקרוא או לכתוב קובץ מסוים.
**פרמטרים חסרים**: אם המשתמש לא הזין את כל הפרמטרים הנדרשים.

---

## הפעלה והרצה

1. פתח את הטרמינל בתיקיית הפרויקט.
2. הרץ את אחת מהפקודות:
   - איגוד קבצים:
     bash
     fib bundle --output bundleFile.txt --language all --sort name --note --author "Your Name"
        - יצירת קובץ תגובה:
     bash
     fib create-rsp
     3. ודא שהקובץ נוצר בהצלחה בתיקיית היעד.

---

## דוגמה לפלט
text
// Author: John Doe
// Source: src/Program.cs
// --- Start of src/Program.cs ---
using System;

class Program {
    static void Main() {
        Console.WriteLine("Hello World");
    }
}
// --- End of src/Program.cs ---

// Source: src/utils/Helpers.cs
// --- Start of src/utils/Helpers.cs ---
namespace Utils {
    public class Helpers {
        public static void SayHi() {
            Console.WriteLine("Hi");
        }
    }
}
