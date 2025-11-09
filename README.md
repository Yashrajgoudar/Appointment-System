# 🏥 Appointment Booking & Reminder Platform

A **cloud-native microservices SaaS** built with **.NET 8**, **RabbitMQ**, **Redis**, and **PostgreSQL** for managing bookings, payments, and automated reminders.  
Deployed using **Aspire** on **Azure Kubernetes Service (AKS)** with **Terraform** and **GitHub Actions CI/CD**.  
Designed for scalability, reliability, and observability — a real-world demonstration of modern DevOps and microservices practices.

---

## 🚀 Features

- Multi-tenant booking and scheduling for small businesses (clinics, salons, tutors)
- Automated payment and reminder notifications via RabbitMQ
- Secure authentication and role-based access using JWT
- Event-driven architecture for asynchronous communication
- Blue-Green deployments and zero downtime using Aspire
- Full observability stack with Prometheus, Grafana, and OpenTelemetry
- Secrets managed securely via Azure Key Vault
- Infrastructure provisioned with Terraform

---

## 🧱 Architecture Overview

**Microservices:**
- `Auth Service` – Handles registration, login, JWT & refresh tokens  
- `Booking Service` – Core scheduling logic and appointment state management  
- `Notification Service` – Event-driven email/SMS reminders (RabbitMQ)  
- `Payment Service` – Payment link generation and webhook handling  
- `Calendar Service` – Manages availability, time zones, and staff slots  

**Shared Infrastructure:**
- PostgreSQL (Data store)  
- Redis (Cache)  
- RabbitMQ (Event bus)  
- Azure Blob Storage (File storage)  
- Prometheus + Grafana + Loki (Monitoring)  

---

## ⚙️ Tech Stack

**Backend:** .NET 8, ASP.NET Core, EF Core  
**Architecture:** Microservices, Clean Architecture, DDD, CQRS  
**DevOps:** Docker, Aspire, Kubernetes (AKS), Helm, Terraform, GitHub Actions  
**Database:** PostgreSQL, Redis  
**Messaging:** RabbitMQ  
**Security:** JWT, Azure Key Vault, HTTPS  
**Monitoring:** Prometheus, Grafana, OpenTelemetry  

---

## 🛠️ Getting Started

### Prerequisites
- .NET 8 SDK  
- Docker & Docker Compose  
- Terraform  
- Azure account (for Aspire deployment)

### Run Locally
```bash
git clone https://github.com/Yashrajgoudar/Appointment-System.git
cd Appointment-System
docker-compose up --build
