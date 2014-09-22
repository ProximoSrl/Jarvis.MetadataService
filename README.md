Jarvis.MetadataService
======================
Simple metadata provider from .csv files.

Should be used as a blueprint for system integrators.

Current implementation load the *.csv files on App_Data in memory with the following structure.


    +provider
    |
    +---+file_name
        |     
        +---+key
            |
            +---property:value
            |
            +---property:value
            |
            +---property:value
