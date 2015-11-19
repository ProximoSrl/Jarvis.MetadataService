Jarvis.MetadataService <img src="http://demo.prxm.it:8811/app/rest/builds/buildType:JarvisMetadataService/statusIcon" alt="build status">
======================

Simple metadata provider from .csv files.

Should be used as a blueprint for system integrators.

Current implementation load the *.csv files on App_Data in memory with the following structure.


    +provider
    |
    +---+file_name (kind)
        |     
        +---+key
            |
            +---property:value
            |
            +---property:value
            |
            +---property:value


The document key is the first column in the .csv file.
Field separator is ;

Schema of the metadata can be queried with:

	GET http://host:port/metadata/kind/@schema

Metadata can be queried with:

    GET http://host:port/metadata/kind/key

or

    POST http://host:port/metadata/query
    {
	    kind:'kind',
	    key:'key'
    }
