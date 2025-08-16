# 🏥 Hospital Project

> **Hospital management system** with role-based access control, appointment management, medical records, and payment tickets.

---

## 📌 Functional Requirements

### 🔑 1. Authentication & Role-Based Access Control
The system must provide a login mechanism with role-based permissions.  
**Supported Roles:**
- 👑 **Admin**
- 🧑‍⚕️ **Head Doctor (Primary)**
- 👩‍⚕️ **Head Nurse (Responsible Nurse)**
- 🧑‍💻 **Standard Users**
- 🩺 **Doctor**
- 💉 **Nurse**
- 🧍 **Patient**

---

### 🧑‍⚕️ 2. Head Doctor (Primary)
- ✅ Full CRUD on **Doctor / Nurse / Patient** within their department  
- 📅 Full management of patient appointments  
- 📄 Update medical records of all patients  
- 💳 Create payment tickets for patients  

---

### 🩺 3. Doctor
- ✅ Full CRUD on **their own appointments**  
- 📄 Update medical records of patients they have treated  
- 💳 Create payment tickets for patients they have visited  

---

### 👩‍⚕️ 4. Head Nurse (Responsible Nurse)
- ✏️ CRUD limited to **Nurse / Patient** within their department  
- 📄 Update medical records of all patients in the department  

---

### 💉 5. Nurse
- 📄 Update medical records of patients they have **assisted or visited**  

---

### 🧍 6. Patient
Can view:  
- 📄 Their own **medical records**  
- 📅 Their own **appointments**  
- 💳 Their own **payment tickets** (both paid and pending)  

