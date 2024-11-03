
### String
```c#
String str = string.Empty;
str.Any();
str.IsNullorEmpty();

str += …;

string phrase = "The quick brown    fox     jumps over the lazy dog.";
string[] words = phrase.Split(' ');

string.Join(‘,’, words);

public string Insert(int startIndex, string value)

public string Remove (int startIndex, int count);

Substring(int startIndex, int length);
```
!!! string cannot use switch(…) {}

### IEnumerable<T>
```c#
System.Collections
System.Collections.Generic

IEnumerable<T>
IEnumerable<T>  = .Concat(IEnumerable<T> …)
IEnumerable<T>  = .Append(T …)
```

### List
```c#
Add(T …)
AddRange(IEnumerable<T> …)
```


### Dictionary<T, T>
```c#
Dictionary<string, string> openWith = new Dictionary<string, string>();
openWith.Add("txt", "notepad.exe");
openWith.Remove("doc");
openWith.Clear
if (!openWith.ContainsKey("doc"))

openWith.Keys
openWith.Values
```

### HashSet<T>
```c#
HashSet<int> oddNumbers = new HashSet<int>();
oddNumbers.Add(…);
```

### Hashtable
```c#
Hashtable openWith = new Hashtable();
openWith.Add("txt", "notepad.exe");
Remove("doc");
openWith.Remove("doc");
openWith.Clear
if (!openWith.ContainsKey("doc"))
```
但沒有Keys跟Values 且 取出記得轉型


### Stack<T>
```c#
Stack<string> numbers = new Stack<string>();
numbers.Push("one");
… = numbers.Pop();
numbers.Peek();
numbers.Count;
numbers.Clear();

number.Contains("four");
```

### Queue
```c#
Queue myQ = new Queue();
myQ.Enqueue("Hello”);
… = myQ.Dequeue();
myQ.Peek();
myQ.Count;
myQ.Clear();
```
