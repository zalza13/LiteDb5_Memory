# 🧟 LiteDb5 Partial Copy

This folder contains **isolated portions of the LiteDB 5 source code** – copied and pasted *shamelessly* – just to make the `MemoryCache` stress test work independently.

## ⚠️ Disclaimer

> **These are NOT meant to be reused or referenced as a full LiteDB implementation.**

They are **bare minimum snippets** extracted from [LiteDB v5 GitHub repo](https://github.com/mbdavid/LiteDB) to allow isolated testing of the memory cache component, and **will likely break** if used in any other context.

## 🧪 Purpose

We use this trimmed-down Frankenstein to:

- Simulate high-concurrency memory cache usage
- Stress-test how `PageBuffer`, `ShareCounter` and memory segments behave
- Verify that no memory leaks occur due to incorrect page reuse

## ✅ If you want the real deal

Please visit the official LiteDB repository:  
👉 [https://github.com/mbdavid/LiteDB](https://github.com/mbdavid/LiteDB)

---
