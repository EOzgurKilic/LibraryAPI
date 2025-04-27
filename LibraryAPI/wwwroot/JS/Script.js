const API_BASE_URL = "https://localhost:44318/api";  // Your correct API base URL

// On page load, don't auto-fetch to avoid errors if API isn't ready
document.addEventListener("DOMContentLoaded", () => {
    console.log("Page Loaded. Ready to fetch data!");
});

// Fetch Authors (GET)
function getAuthors() {
    fetch(`${API_BASE_URL}/Authors`)
        .then(response => {
            if (!response.ok) throw new Error("Failed to fetch authors");
            return response.json();
        })
        .then(data => {
            console.log("Authors Data:", data);  // Debug log
            const authorsList = document.getElementById("authorsList");
            authorsList.innerHTML = "";

            // Handle $values wrapping
            const authors = Array.isArray(data) ? data : data.$values;

            if (!Array.isArray(authors)) throw new Error("Unexpected authors data format!");

            authors.forEach(author => {
                authorsList.innerHTML += `
                    <li class="list-group-item d-flex justify-content-between align-items-center">
                        ${author.name} (ID: ${author.id})
                        <div>
                            <button class="btn btn-sm btn-warning" onclick="updateAuthor(${author.id})">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="deleteAuthor(${author.id})">Delete</button>
                        </div>
                    </li>
                `;
            });
        })
        .catch(error => console.error("Error loading authors:", error));
}


// Fetch Books (GET)
function getBooks() {
    fetch(`${API_BASE_URL}/Books`)
        .then(response => {
            if (!response.ok) throw new Error("Failed to fetch books");
            return response.json();
        })
        .then(data => {
            console.log("Books Data:", data);
            const booksList = document.getElementById("booksList");
            booksList.innerHTML = "";

            const books = Array.isArray(data) ? data : data.$values;

            if (!Array.isArray(books)) throw new Error("Unexpected books data format!");

            books.forEach(book => {
                // Skip $ref entries (they don't have actual data)
                if (book.hasOwnProperty('$ref')) return;

                const title = book.title || "No Title";
                const authorId = book.authorId !== undefined ? book.authorId : "Unknown";

                booksList.innerHTML += `
                    <li class="list-group-item d-flex justify-content-between align-items-center">
                        ${title} (Author ID: ${authorId})
                        <div>
                            <button class="btn btn-sm btn-warning" onclick="updateBook(${book.id})">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="deleteBook(${book.id})">Delete</button>
                        </div>
                    </li>
                `;
            });
        })
        .catch(error => console.error("Error loading books:", error));
}


// Add Author (POST)
function addAuthor() {
    const name = document.getElementById("authorName").value.trim();
    if (!name) return alert("Author name cannot be empty!");

    fetch(`${API_BASE_URL}/Authors`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name })
    })
        .then(response => {
            if (response.ok) {
                getAuthors();
                document.getElementById("authorName").value = "";
            } else {
                alert("Failed to add author.");
            }
        })
        .catch(error => console.error(error));
}

// Add Book (POST)
function addBook() {
    const title = document.getElementById("bookTitle").value.trim();
    const authorId = parseInt(document.getElementById("bookAuthorId").value);

    if (!title || isNaN(authorId)) return alert("Please provide valid book details.");

    fetch(`${API_BASE_URL}/Books`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, authorId })
    })
        .then(response => {
            if (response.ok) {
                getBooks();
                document.getElementById("bookTitle").value = "";
                document.getElementById("bookAuthorId").value = "";
            } else {
                alert("Failed to add book.");
            }
        })
        .catch(error => console.error(error));
}

// Delete Author (DELETE)
function deleteAuthor(id) {
    if (!confirm("Are you sure you want to delete this author?")) return;

    fetch(`${API_BASE_URL}/Authors/${id}`, { method: "DELETE" })
        .then(response => {
            if (response.ok) getAuthors();
            else alert("Failed to delete author.");
        })
        .catch(error => console.error(error));
}

// Delete Book (DELETE)
function deleteBook(id) {
    if (!confirm("Are you sure you want to delete this book?")) return;

    fetch(`${API_BASE_URL}/Books/${id}`, { method: "DELETE" })
        .then(response => {
            if (response.ok) getBooks();
            else alert("Failed to delete book.");
        })
        .catch(error => console.error(error));
}

// Update Author (PUT)
function updateAuthor(id) {
    const newName = prompt("Enter new author name:");
    if (!newName) return;

    fetch(`${API_BASE_URL}/Authors/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, name: newName })
    })
        .then(response => {
            if (response.ok) getAuthors();
            else alert("Failed to update author.");
        })
        .catch(error => console.error(error));
}

// Update Book (PUT)
function updateBook(id) {
    const newTitle = prompt("Enter new book title:");
    const newAuthorId = prompt("Enter new author ID:");
    if (!newTitle || isNaN(parseInt(newAuthorId))) return alert("Invalid input!");

    fetch(`${API_BASE_URL}/Books/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, title: newTitle, authorId: parseInt(newAuthorId) })
    })
        .then(response => {
            if (response.ok) getBooks();
            else alert("Failed to update book.");
        })
        .catch(error => console.error(error));
}
