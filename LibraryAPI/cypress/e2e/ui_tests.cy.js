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

describe('Book and Author Form Tests', () => {
    beforeEach(() => {
        cy.visit('https://localhost:44318/index.html');
    });

    it('Should add a new book and verify both title and Author ID are shown correctly', () => {
        const testTitle = 'Cypress Test Book';
        const testAuthorId = '10';

        cy.get('#bookTitle').clear().type(testTitle);
        cy.get('#bookAuthorId').clear().type(testAuthorId);

        cy.contains('Add Book').click();
        cy.contains('Load Books').click();

        const expectedText = `${testTitle} (Author ID: ${testAuthorId})`;

        
        cy.get('#booksList li').should('contain.text', expectedText);

        
        cy.get('#booksList li').each(($el) => {
            const text = $el.text();
            expect(text).to.not.contain('Nutuk'); 
            expect(text).to.not.contain('Author ID: 999'); 
        });
    });

    it('Should update an existing book and verify updated Title and Author ID', () => {
        cy.visit('https://localhost:44318/index.html');

        cy.contains('Load Books').click();
        

        cy.window().then((win) => {
            cy.stub(win, 'prompt')
                .onFirstCall().returns('Updated Book Title') 
                .onSecondCall().returns('5');              
        });

      
        cy.get('#booksList li').first().contains('Edit').click();

   
        cy.contains('Load Books').click();

       
        cy.get('#booksList li').first().should('contain.text', 'Updated Book Title (Author ID: 5)');
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
        cy.contains('Load Authors').should('be.visible');
        cy.contains('Add Author').should('be.visible');
        cy.contains('Load Books').should('be.visible');
        cy.contains('Add Book').should('be.visible');
    });
    it('Should delete a book and ensure it is removed from the list', () => {
        cy.visit('https://localhost:44318/index.html');


        cy.contains('Load Books').click();


        cy.get('#booksList li').first().invoke('text').then((firstBookText) => {


            cy.get('#booksList li').first().contains('Delete').click();


            cy.contains('Load Books').click();


            cy.get('#booksList li').should('not.contain.text', firstBookText);
        });
});
it('Should add a new author and verify it appears in the list', () => {
    cy.visit('https://localhost:44318/index.html');

    cy.contains('Load Authors').click();

    const newAuthorName = 'Cypress Test Author';

    cy.get('input[placeholder="Author Name"]').clear().type(newAuthorName);
    cy.contains('Add Author').click();

    cy.contains('Load Authors').click();
    cy.get('#authorsList li').should('contain.text', newAuthorName);
});
    it('Should update an author name and verify it is reflected in the list', () => {
        cy.visit('https://localhost:44318/index.html');

        cy.contains('Load Authors').click();

      
        cy.window().then((win) => {
            cy.stub(win, 'prompt').returns('Updated Cypress Author');
        });

        
        cy.get('#authorsList li').first().contains('Edit').click();

        
        cy.contains('Load Authors').click();
        cy.get('#authorsList li').first().should('contain.text', 'Updated Cypress Author');
    });
    it('Should delete an author and verify they are removed from the list', () => {
        cy.visit('https://localhost:44318/index.html');

        cy.contains('Load Authors').click();

        
        cy.get('#authorsList li').first().invoke('text').then((authorText) => {
            
            cy.get('#authorsList li').first().contains('Delete').click();

            
            cy.contains('Load Authors').click();
            cy.get('#authorsList li').should('not.contain.text', authorText);
        });
    });

});


