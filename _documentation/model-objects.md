---
layout: documentation
title: Testing the objects of the model
prev_section: etl-side-effects
next_section: model-exists
permalink: /docs/model-objects/
---
With NBi, you can validate the structure of your model. It can be a SQL, Muldimensional or Tabular model. For each of these models, you can validate the following objects:

| Object | Xml singular | Xml plural | SQL | Multidimensional | Tabular
|-------------|:-----------------:|:-------------------:|:----------------:|
| Table  | table | tables | Yes | - | Yes
| View | view | views | Yes | - | -
| Column | column | columns | Yes | - | Yes
| Perspective | perspective | perspectives | - | Yes | Yes
| Dimension | dimension  | dimensions | - | Yes | Yes
| Hierarchy | hierarchy | hierarchies | - | Yes | Yes
| Level | level | levels | - | Yes | Yes
| Property | property | properties | - | Yes | Yes
| Measure-group | measure-group | mesaure-groups | - | Yes | Yes
| Measure | measure | mesaures | - | Yes | Yes
| Set | set | sets | - | Yes | Yes
| Routine | routine | routines | Yes | - | -
| Routine's parameter | parameter | parameters | Yes | - | -

Routines could be stored procedures, table-valued-functions or scalar-functions.

## System-under test
The sample here under explains how to specify the object that you want to assert. For the list of objects supported by NBi see above.

NBi offers the opportunity to validate that an object exists and is visible through a perspective. To achieve this, you need to stipulate the object that you want to assert in the *syste-under-test*. This is done by creating an xml element named "structure" under the node "system-under-test".
{% highlight xml %}
<test>
    <system-under-test>
        <structure/>
    </system-under-test>
</test>
{% endhighlight %}
In this xml node, named "structure", you'll need to specify on which object you'll perform your assertion. For this you'll need to specify the type of object (perspective, dimension, table ...), its caption and also its parents (expl: for a dimension: a perspective).

In the sample here under, we're writing a system-under-test on a measure named *MyMeasure* in the measure-group *MyMeasureGroup* through the perspective *MyPerspective*
{% highlight xml %}
<structure>
    <measure caption="MyMeasure" measure-group="MyMeasureGroup" perspective="MyPerspective"/>
</structure>
{% endhighlight %}
The tables, here under, describe the parents' information mandatory or optional for each object.

1. SQL and Tabular (Table/columns) objects

| Object/Parent | Schema | Table/View
|-------------|:-----------------:|:-------------------:
| Schema | - | - |
| Table or view | Mandatory | - |
| Column | Mandatory | Mandatory |
| Routine | Mandatory | - |
| Routine's parameter | Mandatory | - |

Routine and routine's parameter are not supported for Tabular models.

For routine's parameter, you can also specify the *direction*

* In
* Out
* In/Out

or if the parameter is a *result* (true/false)

2. Multidimensional and Tabular objects related to dimensions

| Object/Parent | Perspective | Dimension | Hierarchy | Level | Display-Folder|
|--------|:-----------:|:---------:|:---------:|:-----:|:-------------:|
| Perspective | - | - | - | - | - |
| Dimension | Mandatory | - | - | - | - |
| Hierarchy | Mandatory | Mandatory | - | - | Optional |
| Level | Mandatory | Mandatory | Mandatory | - | - |
| Property | Mandatory | Mandatory | Mandatory | Mandatory | - |
| Set | Mandatory | - | - | - | Optional |

3. Multidimensional and Tabular objects related to measures

| Object/Parent | Perspective | Measure-group | Display-Folder|
|--------|:-----------:|:---------:|:---------:
| Measure-group | Mandatory | - | - |
| Measure | Mandatory | Mandatory | Optional |

Finally, the last information to be provided in the *system-under-test* is the connection string to reach your database or cube. This information is specified by the means of the Xml attribute named "connection-string".
{% highlight xml %}
<structure>
    <measure caption="MyMeasure" measure-group="MyMeasureGroup" perspective="MyPerspective"
        connection-string="Provider=MSOLAP.4;Data Source=MyServer;Integrated Security=SSPI;Initial Catalog=MyCube;"/>
</structure>
{% endhighlight %}
Usage of [defaults and references](/docs/config-defaults-references), offers some facilities for the end-users to define more effectively the connection strings for a *test-suite*.
