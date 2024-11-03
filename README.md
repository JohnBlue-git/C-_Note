
### c# data type
![C# DATA TYPE](https://github.com/user-attachments/assets/66550534-fc87-4f67-abe3-e6ae087e6ea1)

### Modfier
- Public
- internal (public but the scope restricted to the same assembly files)
- protected
- private

### Reflection
- Is a tool to detect components in the assembly or dll
- It can use to “unit test” or do “reverse engineering”
- Powerful but have efficiency problem
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileModel
{
    class HelperReflection
    {
        static public void LineParser(Type type, object entity, string line, int lineLen, out bool success)
        {
            if (line.Length != lineLen)
            {
                success = false;
                return;
            }
            else success = true;

            ParseAndSetEntity(type, entity, type.GetProperties().Select(x => x.Name), line);
        }
        static public void ParseAndSetEntity(Type type, object entity, IEnumerable<string> columns, string line)
        {
            int startPos = 0;
            int spanLen = 0;
            string parser;
            foreach (string column in columns)
            {
                spanLen = GetCloumnStrLen(type, column);
                parser = line.Substring(startPos, spanLen);
                parser = string.IsNullOrWhiteSpace(parser) ? string.Empty : parser;
                SetCloumnValue(type, entity, column, parser);
                startPos += spanLen;
            }
        }
        static public int GetCloumnStrLen(Type type, string columnName)
        {
            StringLengthAttribute len = type.GetProperty(columnName).GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
            if (len != null) return len.MaximumLength;
            else return -1;
        }
        static public void SetCloumnValue(Type type, object entity, string columnName, string value)
        {
            type.GetProperty(columnName).SetValue(entity, value);
        }
    }

    // 需要再確定
    // DT_DECIMAL 跟 Decimal
    // DT_STR (非Unicode) 跟 
    // DT_WSTR (Unicode) 跟 

    class tap_mhok_log
    {
        //tseno 	//DT_STR 7 
        //bs        //DT_STR 1
        //term		//DT_STR 1
        //dseq		//DT_STR 4
        //ttime		//DT_STR 8
        //bhno		//DT_STR 1
        //cumb_key	//DT_DECIMAL 7
        //otype		//DT_DECIMAL 1
        //stock		//DT_STR 6
        //mqty		//DT_DECIMAL 8
        //ecode		//DT_DECIMAL 1
        //price		//DT_WSTR 9 
        //tsale		//DT_STR 3
        //tdate		//DT_STR 8
        //amt		//DT_WSTR 12
        //TradeType	//DT_STR 1

        [StringLength(7)] public string tseno { get; set; }

        [StringLength(1)] public string bs { get; set; }

        [StringLength(1)] public string term { get; set; }

        [StringLength(4)] public string dseq { get; set; }

        [StringLength(8)] public string ttime { get; set; }

        [StringLength(1)] public string bhno { get; set; }

        [StringLength(7)] public string cumb_key { get; set; }

        [StringLength(1)] public string otype { get; set; }

        [StringLength(6)] public string stock { get; set; }

        [StringLength(8)] public string mqty { get; set; }

        [StringLength(1)] public string ecode { get; set; }

        [StringLength(9)] public string price { get; set; }

        [StringLength(3)] public string tsale { get; set; }

        [StringLength(8)] public string tdate { get; set; }

        [StringLength(12)] public string amt { get; set; }
        
        [StringLength(1)] public string TradeType { get; set; }

        public tap_mhok_log(string line, out bool success)
        {
            HelperReflection.LineParser(GetType(), this, line, 78, out success);
        }

        static tap_mhok_log()
        {
            int len = 0;
            foreach (var item in typeof(tap_mhok_log).GetProperties())
            {
                len += HelperReflection.GetCloumnStrLen(typeof(tap_mhok_log), item.Name);
            }
        }
    }
}
```


### anonymous
- A type that deosn’t defined its class name
```c#
var anonymousData = new
{
	ForeName = "Jignesh",
	SurName = "Trivedi"
};
```

### 比較運算子(==)跟 Equals()方法的差異
- == 運算子跟 Equals()方法都是用來比較兩個數值型別資料項目或是參考型別的資料項目，等號比較運算子(==)是比較運算子
- 而 Equals()方法比較字串的內容

### Some reference
https://job.achi.idv.tw/2020/11/26/most-asked-c-interview-questions/

### Try
```c#
int.TryParse(line, out int number)
```

### Property : field and function
```c#
public class Student
{
    public string Name { get; set; }
    public double GPA { get; set; }
}

var students = new List<Student>
{
    new Student { Name = "Alice", GPA = 3.6 },
    new Student { Name = "Bob", GPA = 3.2 },
    new Student { Name = "Charlie", GPA = 3.8 }
};
```

### Passing Property

```csharp
static public bool LineParser(string line, Action<string> setProperty, Func<string> getProperty, bool checkNull, ref int startPos, int parseLen)
        {
            if (startPos < 0 || (startPos + parseLen) > (line.Length - 1)) return false;

            setProperty(line.Substring(startPos, parseLen));
            startPos += parseLen;

            if (checkNull && string.IsNullOrWhiteSpace(getProperty())) return false;
            else return true;
        }

sucs = HelperParser.LineParser(line, (value) => { tseno     = value; }, () => { return tseno;     },  true, ref startPos, 7);

```

### List and String
```c#
.Length
.Add
.AddRange
.Remove
.RemoveAt
.RemoveAt(.Count - 1);

https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.add?view=net-8.0
```

### LINQ
is extension of IEnumerable<T>
```c#
using System.Linq;

if(sortAscending) {
    qry = qry.OrderBy(x=>x.Property);
} else {
    qry = qry.OrderByDescending(x=>x.Property);
}

https://learn.microsoft.com/zh-tw/dotnet/csharp/linq/get-started/introduction-to-linq-queries
```
查询表达式语法（Query Expression）和方法语法（Fluent Syntax）
```c#
    // Group
        IEnumerable<string> list = new List<string> {
            "aaa.frx", "bbb.TXT", "xyz.dbf", "abc.pdf", "aaaa.PDF", "xyz.frt", "abc.xml", "ccc.txt", "zzz.txt"
        };
        var res = list
            .GroupBy(x => x.Substring(x.Length - 3, 3).ToLower())
            .Select(x => new {
                name = x.Key,
                count = x.Count(),
            });
        foreach (var item in res) {
            Console.WriteLine($"{item.name}:{item.count}");
        }
        Console.WriteLine("");
        
    // OrderBy
        IEnumerable<string> list = new List<string>{"abc", "bc", "c"};
        IEnumerable<string> res = list.OrderBy(x => x.Length);
        foreach (var item in res) {
            Console.Write(item);
        }
        Console.WriteLine("");


    // ToDictionary
	.ToDictionary(
	    x => x.Key
	    , x => x.Sum(xv => xv.Age)
	); 
```
.First() and .FirstOrDefault()
```c#
```


### Dapper (extension)
ORM，英文叫Object Relational Mapping，翻譯成中文為物件關聯對映。 ORM 在網站開發結構中，是在『資料庫』和『 Model 資料容器』兩者之間，
```c#
using Dapper;

.AsList();
```

### Task
``` c#
// Task
Task hello ＝new Task(() => { … });
hello.Start();
hello.Wait();
也可await hello;

// 回傳Task + Run
Task hello() { return Task.Run(() => { …; Thread.Sleep(3000); }); }
hello().Wait();

// 回傳 接收
td = hello(); 回傳是 Task<T>
td.Result; 有使用 會自動await

// 回傳接收 Async Await
// 可以type
int num = await hello(); 

// 修飾 async Task 跟 await
await hello;
await hello();

// 一定要有async 不然要回傳Run
async Task hello() { ...; return; }
async Task<T> hello() { ...; return T; }

// async Task<T> 內一定要有 Task.Run 或 其他async Task
// 不然使用起來跟同步一樣了
async Task<T> hello() { await Task.Run(…); }
```

### Handle Task Exception
```csharp
try {
	// wait for all tasks in the list
	Task.Run(async () => await Task.WhenAll(taskList)).Wait();
}
catch (AggregateException ae)
{
    string message = "";
    foreach (var tsk in taskList) // ae.InnerExceptions
    {
        message += $"job class: {this.GetType().Name}; {tsk.Exception}\n\n";
    }
    throw new Exception($"{message}");
}
catch (Exception ex)
{
    throw new Exception($"job class: {this.GetType().Name}; {ex.ToString()}\n");
}
```


### Implementing a Singleton Pattern

Task:
- Implement a thread-safe Singleton class in C#.

Constraints:
- The Singleton class should be designed in such a way that only a single instance of the class can exist in the application, and this instance should be accessible globally.

```c#
public sealed class Singleton
{
    private static Singleton instance = null;
    private static readonly object padlock = new object();
    Singleton() {}
    public static Singleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
```

### Use StringBuilder for concatenation in tight loops.
var sb = new System.Text.StringBuilder();
for (int i = 0; i < 20; i++)
{
    sb.AppendLine(i.ToString());
}
System.Console.WriteLine(sb.ToString());

### File and Authencation
```csharp
https://learn.microsoft.com/zh-tw/dotnet/api/system.io.filestream.-ctor?view=net-8.0#system-io-filestream-ctor(system-string-system-io-filemode-system-io-fileaccess-system-io-fileshare-system-int32)

FileShare
Contains constants for controlling the kind of access other operations can have to the same file.

using (var sr = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
{
    // etc...
}
```

### Alias
```csharp
using SysTask = System.Threading.Tasks;
```

