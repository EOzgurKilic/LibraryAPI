describe('LibraryAPI UI Tests', () => {

  it('should load the homepage correctly', () => {
    cy.visit('https://localhost:44318/index.html');
    cy.contains('Library Management').should('be.visible');
  });

  it('should add a new author with valid input', () => {
    cy.visit('https://localhost:44318/index.html');

    cy.get('input[placeholder="Author Name"]').type('Test Author');
    cy.contains('Add Author').click();

    cy.contains('Load Authors').click();
    cy.contains('Test Author').should('be.visible');
  });

  it('should not add an author with empty input', () => {
    cy.visit('https://localhost:44318/index.html');

    cy.window().then((win) => {
      cy.stub(win, 'alert').as('alertStub');
    });

    cy.get('#authorName').clear();
    cy.contains('Add Author').click();

    cy.get('@alertStub').should('have.been.calledWith', 'Author name cannot be empty!');
  });

});

// ➡️ Add Book Form Tests Below
describe('📚 Book Form Tests', () => {

  beforeEach(() => {
    cy.visit('https://localhost:44318/index.html');
  });

  it('Should add a new book with valid input', () => {
    cy.visit('https://localhost:44318/index.html');

    // Fill in book details
    cy.get('#bookTitle').clear().type('Cypress Test Book');
    cy.get('#bookAuthorId').clear().type('10');   // Ensure Author ID 10 exists or just type here the id of an existing author in the DB

    cy.contains('Add Book').click();

    // Reload the books list
    cy.contains('Load Books').click();

    // Explicitly wait for list items and check for the book
    cy.get('#booksList li')
        .should('exist')  // Wait until at least one item exists
        .contains('Cypress Test Book')  // Look for the specific book
        .should('be.visible');  // Ensure it's visible
  });



  it('Should not add a book with invalid input (empty fields)', () => {
    cy.get('#bookTitle').clear();
    cy.get('#bookAuthorId').clear();

    cy.window().then((win) => {
      cy.stub(win, 'alert').as('bookAlert');
    });

    cy.contains('Add Book').click();

    cy.get('@bookAlert').should('have.been.calledWith', 'Please provide valid book details.');
  });

  it('Should display all main buttons on page load', () => {
    cy.visit('https://localhost:44318/index.html');

    cy.contains('Load Authors').should('be.visible');
    cy.contains('Add Author').should('be.visible');
    cy.contains('Load Books').should('be.visible');
    cy.contains('Add Book').should('be.visible');
  });

});
