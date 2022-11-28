const d = new Date();
function formatDate(date, format) {
    const map = {
        mm: date.getMonth() + 1,
        dd: date.getDate(),
        yy: date.getFullYear().toString().slice(-2),
        yyyy: date.getFullYear(),
    }
    return format.replace(/mm|dd|yy|yyy/gi, matched => map[matched])
}
formatDate(d, 'mm/dd/yy');
var nombre='testCypress'+ d.toString();
var evento='evento'+ d.toString();
evento=evento.replace(/\s+/g, '').substring(0,10);


/*describe('RegistroUserTest', ()=>{
    it('RegistroSuccess', ()=>{
        cy.visit(Cypress.env("base_url"));
        cy.get('#register').click()
        cy.get('input[name="nombre"]').type(nombre)
        cy.get('input[name="apellidos"]').type(nombre)
        cy.get('input[name="email"]').type(nombre+'@cypress.pe')
        cy.get('input[name="password"]').type('prueba')
        cy.get('#inlineRadio1').click()
        cy.get('#registerSubmit').click()
        cy.get('.alert').should('contain.text', 'Usuario registrado con exito')
    })
    
    it('Registro Fail', ()=>{
        cy.visit(Cypress.env("base_url"));
        cy.get('#register').click()
        cy.get('input[name="nombre"]').type('Prueba')
        cy.get('input[name="apellidos"]').type('Prueba')
        cy.get('input[name="email"]').type('admin@admin.pe')
        cy.get('input[name="password"]').type('prueba')
        cy.get('#inlineRadio1').click()
        cy.get('#registerSubmit').click()
        cy.get('.alert').should('contain.text', 'Lo sentimos, el correo electrónico ya existe en nuestra base de datos')                             
    })
});*/

describe('LoginTest', ()=>{
    it('Login fail validation', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('evento@prueba.com')
        .should('have.value', 'evento@prueba.com')
        cy.get('#Input_Password').type('123')
        .should('have.value', '123')
        cy.get('#login-submit').click()
        cy.get('.alert').should('contain.text', 'No tienes credenciales correctas')
    })
    
    it('Login correct validation', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('admin@admin.pe')
        cy.get('#Input_Password').type('admin')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido Admin!')
    })
});

describe('MiPerfil', ()=>{
    it('ViewPerfil', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('admin@admin.pe')
        cy.get('#Input_Password').type('admin')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido Admin!')
        cy.get('.navbar-toggler-icon').click()
        cy.get('perfil')
    })
});

describe('Ver Perfil', ()=>{

    it('PerfilPromotor', ()=>{
    cy.visit(Cypress.env("base_url")+'/Promotor/Login');
    cy.get('#Input_Email').type('brandon.rodriguez.bmrc@outlook.es')
    cy.get('#Input_Password').type('admin')
    cy.get('#login-submit').click()
    cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido Brandon')
    cy.get('.navbar-toggler-icon').click()
    cy.get('#perfil').click()
    cy.get('#headingOne').click()
    cy.get('#button1').click()
    cy.get('#numero').should('contain.text', '#')
    })
    it('PerfilEventos', ()=>{
    cy.visit(Cypress.env("base_url")+'/Promotor/Login');
    cy.get('#Input_Email').type('brandon.rodriguez.bmrc@outlook.es')
    cy.get('#Input_Password').type('admin')
    cy.get('#login-submit').click()
    cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido Brandon')
    cy.get('.navbar-toggler-icon').click()
    cy.get('#perfil').click()
    cy.get('#headingTwo').click()
    cy.get('#button2').click()
    cy.get('#fecha').should('contain.text', 'FECHA')
    })
    it('EventosDistantes', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('brandon.rodriguez.bmrc@outlook.es')
        cy.get('#Input_Password').type('admin')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido Brandon')
        cy.get('.navbar-toggler-icon').click()
        cy.get('#perfil').click()
        cy.get('#headingThree').click()
        cy.get('#button3').click()
        cy.get('#Evento1').should('contain.text', 'Fongal Cajamarca')
        })
});