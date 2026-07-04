# RAG Web API

A sample application demonstrating AI integration with Angular and ASP.NET Web API. It covers document search, image search/generation, movie search, and agentic task/email management.

## Overview

This project showcases:
- **Document Search** and **Bulk Insert**
- **Image Search**, **AI Insight**, and **Bulk/Single Insert**
- **Movie Bulk Insert** and **Search**
- **Agentic AI** for task management and email

All three core modules — Documents, Images, and Movies — include a "Start Analysis" feature that computes float embeddings for semantic search.

## Agentic AI

This project includes an agentic AI feature for managing tasks with a limited command set: add, list, delete, mark complete, and send email.

On the backend, this is implemented using two specialized agents:
- **TaskAgent** — handles task-related commands (add, list, delete, mark complete)
- **EmailAgent** — handles sending emails

Both agents are coordinated by a **SupervisorAgent**, which interprets user prompts and routes them to the correct agent.

## Modules

### Images

Two pages:
- **Image Insertion, List, and Semantic Search**
- **Image Generation**

**Image Insertion, List, and Semantic Search**
Insert images into the database, trigger image embeddings and image insight embeddings after insertion, and view the list of images. Includes a slideshow view and per-image insight on click, plus semantic search that computes similarity against the query and displays relevant images.

**Image Generation**
Generate images based on a text prompt.

### Documents

Two pages:
- **Document Insertion and List**
- **Chat Bot**

**Document Insertion and List**
Insert documents into the database, trigger document embeddings after insertion, and view the list of documents.

**Chat Bot**
Computes similarity between the user query and all documents, then sends the query, prior conversation history, and the retrieved documents to OpenRouter to generate an answer.

### Movies

Two pages:
- **Movies List, Paging, and Semantic Search**
- **Config Movies**

**Movies List, Paging, and Semantic Search**
Displays movies page by page, with search functionality. Search computes similarity between the user query and movies in two ways:
1. Separate embeddings per field, combined using per-field weights
2. A single embedding computed against a combined string of movie details

**Config Movies**
Configure the weights of different fields used in search, insert movies into the database, and trigger movie embedding calculations.

## Tech Stack

- **Backend:** ASP.NET Core Web API
- **Frontend:** Angular
- **AI/Agentic Layer:** Agent-based architecture (SupervisorAgent, TaskAgent, EmailAgent)

## Environment Setup

Copy `.env.example` to `.env` in the project root and fill in your own values:

```env
# DATABASE PROPERTIES
DBSERVER=<Database Server Name>
DBNAME=RagContext
DBUSER=<Database User>
DBPASSWORD=<Database Password>

# AI KEYS
JINA_KEY=<Jina Key>
OPEN_ROUTER_KEY=<Open Router Key>
CLOUD_FLARE_KEY=<Cloud Flare Key>
MISTRAL_KEY=<Mistral Key>

ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:4100
ASPNETCORE_USE_HTTP=false
DOTNET_RUNNING_IN_CONTAINER=false
```

### Variable Reference

| Variable | Description |
|---|---|
| `DBSERVER` | SQL Server hostname/address |
| `DBNAME` | Database name (default: `RagContext`) |
| `DBUSER` | SQL Server username |
| `DBPASSWORD` | SQL Server password |
| `JINA_KEY` | API key for Jina (used for embeddings/reranking) |
| `OPEN_ROUTER_KEY` | API key for OpenRouter (used by the document Chat Bot) |
| `CLOUD_FLARE_KEY` | API key for Cloudflare (used for image generation) |
| `MISTRAL_KEY` | API key for Mistral AI (used by the agentic AI feature) |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment (`Development`/`Production`) |
| `ASPNETCORE_URLS` | URL/port the API listens on |
| `ASPNETCORE_USE_HTTP` | Toggles HTTP vs HTTPS binding |
| `DOTNET_RUNNING_IN_CONTAINER` | Set to `true` when running inside a Docker container |

> **Note:** Never commit your `.env` file. Only `.env.example` (with placeholder values) should be tracked in the repository.
