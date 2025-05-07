# DBAudit


**DBSonar** is an open-source tool that analyzes the quality, performance, and security of relational databases – similar in concept to [SonarQube](https://www.sonarqube.org/) but focused entirely on databases.

The tool helps developers, DBAs, and data architects identify potential issues in their database schema, stored procedures, indexing strategy, and access control. It supports continuous improvement by providing clear metrics, quality gates, and recommendations.

---

## 📌 TODO (Planned Features & Metrics)

### ✅ Structural Analysis
- [ ] Detect tables without primary keys
- [ ] Detect tables without indexes
- [ ] List columns with nullable types without justification
- [ ] Flag tables with excessive column counts
- [ ] Identify tables without foreign key relationships
- [ ] Report on use of `TEXT`/`BLOB` types without real need
- [ ] Identify undocumented tables and columns

### 🚀 Performance Metrics
- [ ] Identify unused or duplicate indexes
- [ ] Detect missing indexes on frequently queried columns
- [ ] Flag overly large composite indexes
- [ ] Benchmark sample queries for execution time
- [ ] Detect fragmented indexes
- [ ] Measure data-to-index size ratio

### 🔐 Security Checks
- [ ] Detect sensitive data columns stored without encryption
- [ ] Identify database users with excessive privileges
- [ ] Detect tables without appropriate access controls
- [ ] Flag procedures/functions that use unsafe dynamic SQL

### 📏 Standards & Consistency
- [ ] Check for naming convention violations (e.g., snake_case vs camelCase)
- [ ] Flag inconsistent data types for similar fields (e.g., phone number as `TEXT` vs `VARCHAR`)
- [ ] Detect unformatted or non-standard SQL
- [ ] Identify duplicated reference data across tables

### 🧠 Code Quality (SQL Scripts)
- [ ] Flag procedures without unit tests
- [ ] Highlight high-complexity stored procedures/functions
- [ ] Identify unused views, procedures, or functions
- [ ] Detect use of `SELECT *` in views or procedures
- [ ] Check for missing comments and documentation

### 📊 Evolution & Change Tracking
- [ ] Track schema change frequency (schema churn)
- [ ] Monitor number of migration files (e.g., Flyway/Liquibase)
- [ ] Count rollback operations (indicates migration quality issues)

---

## 🔍 Future Ideas
- [ ] Visual dashboard for database health
- [ ] CI/CD integration for schema reviews
- [ ] Plugin system for supporting various SQL dialects (PostgreSQL, MySQL, SQL Server, etc.)
- [ ] GitHub Action for database linting in pull requests

---
## Links:
https://www.indiehackers.com/product/dbaudit


## 📫 Contributing

Feature suggestions, bug reports, and PRs are welcome!  
Want to be part of shaping DBSonar? Let's talk!

---
