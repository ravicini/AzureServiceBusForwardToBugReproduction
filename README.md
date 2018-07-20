# Introduction

This repository is to reproduce a forward to bug found in Azure Service Bus on premium tier.
(Requires .net 471)

## How to Setup the Namespace

1. create a new namespace with 1 Message Unit and Premium tier
2. create a queue (name: `queue`)
    - Enable batched operations
    - Enable dead lettering on message expiration
    - 14 days default time to live
    - 10 GB max queue size
    - 3 max delivery count
    - Disable session
3. create a topic (name: `topic`)
    - Enable batched operation
    - 14 days default time to live
    - 10 GB max size
    - Disable session
4. create Subscription (name: `forward`) on `topic`
    - Enable dead lettering on message expiration
    - 3 max delivery count
    - Forward To: `queue`
    - Enable batched operations
    - Disable session

## How to start the Retrigger app

Configure the Retrigger console app with the `appsettings.json` file:

```
{
    "servicebusConnection": "INSERT CONNECTION STRING HERE",
    "servicebusBatchSize": 700,
    "queue": "topic"
}
```

Let the application send messages to the topic -> they'll get auto forwarded to the queue, after ~1 hour the forwarding mechanism stops working.
You can see the bug occured if:

- Subscription Queue length starts to grow a lot
- Queue length stops to grow