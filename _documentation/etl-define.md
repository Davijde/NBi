---
layout: documentation
title: Define an etl
prev_section: query-shared-dataset
next_section: etl-parameters-variables
permalink: /docs/etl-define/
---
**Important remarks:**

* Currently (will be changed in a near future) NBi *only* supports testing etls' runs **on the same server** than the tests. It means that it’s not possible to run your tests on your own computer (or on your build server) and try to execute remotely an SSIS package.
* This version NBi supports Sql Server 2008R2, Sql Server 2012, Sql Server 2014 and Sql Server 2016.
* **Only SSIS packages** can be executed (Support for others Etl is not planned)

Etl could be defined in two placeholders: the setup and the system-under-test.

## Define an etl

To achieve this, you’ll need to define your tested etl in the system-under-test (or the setup) and specify its name in the corresponding xml attribute (the extension dtsx is not needed):

{% highlight xml %}
<system-under-test>
  <result-set>
    <etl name="myPackage" >
    </etl>
  </result-set>
</system-under-test>
{% endhighlight %}

If your package is not running on the default version of Sql Server (currently SqlServer2014), you can specify it on the element *etl* by the means of the attribute *version*. The valid values for his attribute are:

* SqlServer2016
* SqlServer2014
* SqlServer2012
* SqlServer2008R2

{% highlight xml %}
<etl 
    name="myPackage" 
    version="SqlServer2012"
>
</etl>
{% endhighlight %}

To be testable a package can be hosted on:

* Files folder
* SQL Server
* SSIS Catalog

### Hosted on a files folder
To specify an etl available on a file folder, you must define its path in the corresponding xml attribute named *path*.  Note that this path is **relative** to your test-suite and should end by a backslash.
{% highlight xml %}
<etl path="relative-folder\" name="myPackage"/>
{% endhighlight %}

You can also provide an absolute path starting by the letter of a drive.
{% highlight xml %}
<etl path="C:\absolute-folder\" name="myPackage"/>
{% endhighlight %}

### Hosted on SQL Server
If you want to specify that your package is hosted on a SQL Server, then specify the xml attribute named *server* with the name of your SQL Server. The attribute *path*  is relative to the root of your SSIS Server and should start and end by a slash.

{% highlight xml %}
<etl server="." path="/SSIS/" name="myPackage"/>
{% endhighlight %}

Some packages could be encrypted, in this case it's needed to provide the password. It can be done using the xml attribute name *password*.

{% highlight xml %}
<etl
    server="."
    path="/SSIS/"
    name="myPackage"
    password="p@ssw0rd"
/>
{% endhighlight %}


If you want to run your package in the context of a specific user, you must specify the xml attributes *username* and *password*.

{% highlight xml %}
<etl
    server="."
    path="/SSIS/"
    name="myPackage"
    username="myusername"
    password="p@ssw0rd"
/>
{% endhighlight %}

### Hosted on SQL Server (SSIS Catalog)
You can also access the SSIS Catalog provided with SQL Server version 2012 and above using the xml attributes named *catalog*, *folder* and *project* to respectively define the name of this SSIS Catalog, the name of the folder and the name of your SSIS project.

{% highlight xml %}
<etl
    server="localhost"
    catalog="SSISDB"
    folder="Folder"
    project="MyProject"
    name="package.dtsx"
 />
 {% endhighlight %}

Timeout
-------
For the packages stored on a SQL Server in the SSIS Catalog, it's possible to overwrite the default timeout of 30 seconds. To achieve this, just specify an xml attribute named _timeout_ and its value in milli-seconds. Note that the way the package is started is slightly different to achieve this feature, use this only when needed. In the sample here under, the timeout is set to one minute.

{% highlight xml %}
<etl
    server="localhost"
    catalog="SSISDB"
    folder="Folder"
    project="MyProject"
    name="package.dtsx"
    timeout="60000"
 />
 {% endhighlight %}

Define tests
------------
 Currently, it’s possible to define three kinds of tests for an Etl:

 * validate that a run is [successful](../etl-successful)
 * validate the [execution timespan for a package](../etl-performance)
 * validate the [side effects](../etl-side-effects) of a run

 Note that last kind of test is fundamentally different. Indeed, the system-under-test is not the etl but the object on which you want to observe a change after the run of your Etl. This kind of test is detailed on the page [test side effects of an etl](../etl-side-effects)
