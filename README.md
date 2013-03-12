duhmain
=======

A simple command, query, controller, service, entity, and event pattern.

Main Concepts
-------------
###Entity
Something with an identifier that is unique in its context. 
A _VersionedEntity_ is an entity with an opaque version tag (like an ETag).

###Command
An operation that explicitly has a side-effect on the system. Commands have results. 

###Boundary Service
An explicit boundary for the system (like network, disk, database, or another context).
Services expose methods and can raise events to controllers.
Services are located by Queries, Commands, and Controllers via a _ServiceLocator_

###Controller
A coordinator for Commands and Queries. Also recieves stimuli (events) from a boundary.

###Context
A container for ambient information about the context that a Command or Query runs in.

To Be Done
----------
###Query
An operation that returns a single result and has no side-effect on the system.
A _MultiResultQuery_ is a query that returns a sequence of data items.
A _PagedMultiResultQuery_ is a query that supports a paging pattern for the sequence of results it returns. 

###Repository
A type of Boundary service for Getting, Saving, and Querying entities.

###Event
A stimilus from a boundary service.

###Asynchronous Commands and Queries
Does what the heading says on the tin.

###Unit of Work
A batch of Commands to be executed together in the same context.
