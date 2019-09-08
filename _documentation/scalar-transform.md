---
layout: documentation
title: Transform a scalar
prev_section: primitive-result-set
next_section: scalar-native-transformation
permalink: /docs/scalar-transform/
---
It happens that it's impossible (stored procedures) or not suitable (readibility of the test) to tune the query in your assertion to match with the expectations of your *system-under-test*. The feature *transform* is built for these cases.

The element *transform* works as a modification of the content of all cells (scalar) for a given column.

## Languages

At the moment, you can make usage of three different languages and a few ready-to-go transformations provided by the framework to define your transformation:

* NCalc - ```ncalc```
* Format - ```format```
* C# - ```c-sharp```
* Native - ```native```

These options support different purposes:

*NCalc* is specifically dedicated to calculations on numerical values but also offers limited features for booleans and texts.

*Format* is a quick way to transform a *dateTime* or *numeric* to a text representation taking into account culture specific formats (leading 0, 2 or 4 digits for years, coma or point for decimal spearator and many more).

*C#* is there to support advanced transformations requiring a little bit more of code.

*Native* is a collection of ready-to-go transformations for general purposes. It's especially useful around *null*, *empty*, *blank* and *text*

| Language | source | destination
| ----------------------------
| NCalc | numeric (limited for boolean and text)| unchanged
| Format | numeric or dateTime | text
| C# | all | all
| Native | text, numeric or dateTime | text, numeric or dateTime

Technically, the destination could be something else that the option(s) defined in the table above, but most of the time it has no added-value.

The content of the cell is always casted on .NET based on the information provided into the attribute *original-type*

### NCalc

The whole documentation of this language and the supported syntax is available on the website of this project
(especially [here](https://ncalc.codeplex.com/wikipage?title=functions&referringTitle=Home) and [there](https://ncalc.codeplex.com/wikipage?title=operators&referringTitle=Home)).

The cell's content is available through the variable *value*.

The exemple here under is transformating the content of two columns:

* The first column is hosting a *numeric* that will be multiplied by 1.21 
* The second column is hosting a *numeric* that will be divided by 1000 before rounded with 2 digits.

{% highlight xml %}
<assert>
  <equal-to>
    <column index="1" role="value" type="text">
      <transform language="ncalc" original-type="numeric">value * 1.21</transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="ncalc" original-type="numeric">Round(value/1000, 2)</transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}

### Format

The syntax is exactly the one supported by the *Format* function and is available [for dateTime](https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx) and [numeric](https://msdn.microsoft.com/en-us/library/0c899ak8(v=vs.110).aspx)

The exemple here under is transformating the content of three columns:

* The first column is hosting a *dateTime* that will be converted to a year/month representation: 4 digits for years, 2 for months and seperated by a dot.
* The second column is hosting a *numeric* that will be converted to a currency representation: a euro symbol followed by 3 digits before the decimal separator (a coma in this case because we use the french culture) and 2 digits after this seperator 
* The third column is hosting a *numeric* that will be converted to a currency representation but this time using k€ (thousand of euros, the cell content with be divided by 1000).

{% highlight xml %}
<assert>
  <equal-to>
    <column index="0" role="key" type="text">
      <transform language="format" original-type="dateTime">yyyy.MM</transform>
    </column>
    <column index="1" role="value" type="text" culture="fr-fr">
      <transform language="format" original-type="numeric">€000.00</transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="format" original-type="numeric">k€000,</transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}

### C-Sharp

Currently, it's limited to one line of code (one expression) but later you'll be able to call functions defined in an assembly or directly write a few lines of code in your test's definition. The syntax is the syntax supported by C# 5.0 without a terminator (;) at the end of the expression.

The cell's content is available through the variable *value*.

The exemple here under is transformating the content of two columns:

* The first column is hosting a *dateTime* that will be manipulated to add a month and extract the year and month
* The second column is hosting a *numeric* that will be multiplied by 1.21 after taking the absolute value
* The third column is hosting a *numeric* that will be divided by 1000 before rounded with 2 digits, then we'll add the symbols k€ in front of the result.

{% highlight xml %}
<assert>
  <equal-to>
    <column index="0" role="key" type="text">
      <transform language="c-sharp" original-type="dateTime">
        value.AddMonth(1).Year.ToString() + "." + (value.AddMonth(1).Month.ToString()
      </transform>
    </column>
    <column index="1" role="value" type="text">
      <transform language="c-sharp" original-type="numeric">
        Math.Abs(value * 1.21)
      </transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="c-sharp" original-type="numeric">
        value < 5000 ? string.Format(€{0:##00.00}, value) : "k€" + Math.Round(value/1000, 2).ToString()
      </transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}

### Native

It's possible to define one or more straighforward [native transormations](../scalar-native-transformation) embeded in the NBi framework.

It's possible to chain the transformation but not apply a native transformation to the parameter of another native transformation. To chain transformation you must separate them by a pipe (```|```).

{% highlight xml %}
<assert>
  <equal-to>
    <column index="0" role="key" type="text">
      <transform language="native" original-type="text">
        text-to-trim | text-to-upper
      </transform>
    </column>
    <column index="1" role="value" type="text">
      <transform language="native" original-type="text">
        empty-to-null
      </transform>
    </column>
    <column index="2" role="value" type="text">
      <transform language="native" original-type="text">
        any-to-value
      </transform>
    </column>
    <column index="3" role="value" type="dateTime">
      <transform language="native" original-type="dateTime">
        dateTime-to-clip(2010-01-01, 2019-12-31)
      </transform>
    </column>
  </equal-to>
</assert>
{% endhighlight %}