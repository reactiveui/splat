! This is in a separate file while I work on the PR so I don't have to remerge any readme pieces :)

# Application Performance Monitoring

Application Performance Monitoring is split into the follow sections

* Error Reporting
* Feature Usage Tracking
* View Tracking

The table below shows the support across various APM packages

| Product | Maturity Level | Error Reporting | Feature Usage Tracking | View Tracking |
-------------
| Appcenter| Alpha | TODO | Native | Native |
| Application Insights | Alpha | TODO | Native | Native |
| Exceptionless | Alpha | TODO | Native | By Convention |
| New Relic | Not Started | TODO | TODO | TODO
| Raygun | Prototype | TODO | By Convention | By Convention |

## Goals of the Splat APM feature

* To sit on top of existing APM libaries using native features where possible, or by using a common convention that gives parity in behaviour.
** Where there is a convention behaviour it will be detailed under the relevant frameworks documentation.
* To define basic behaviours that are dropped into consuming libraries, for example with ReactiveUI
** Commands
** ViewModels
** Views

## Getting started with APM with Splat

Splat comes with a default implementation that pushes events into your active Splat logging framework. This allows for design and testing prior to hooking up a full APM offering.

### Error Reporting

TODO

### Feature Usage Tracking

TODO

### View Tracking

TODO

## Configuring Appcenter

First configure Appcenter. For guidance see TODO

```cs
using Splat.AppCenter;

// then in your service locator initialisation
Locator.CurrentMutable.UseAppcenterApm();
```

## Configuring Application Insights

First configure Application Insights. For guidance see TODO

```cs
using Splat.ApplicationInsights;

// then in your service locator initialisation
Locator.CurrentMutable.UseApplicationInsightsApm();
```

## Configuring Exceptionless

First configure Exceptionless. For guidance see TODO

```cs
using Splat.Exceptionless;

// then in your service locator initialisation
Locator.CurrentMutable.UseExceptionlessApm();
```

## Configuring New Relic

New Relic support isn't currently available.

## Configuring Raygun

First configure Raygun. For guidance see TODO

```cs
using Splat.Raygun;

// then in your service locator initialisation
Locator.CurrentMutable.UseRaygunApm();
```