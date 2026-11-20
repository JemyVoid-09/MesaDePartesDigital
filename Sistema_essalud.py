"""
el import hashlib es una herramienta que nos ayuda a transformar cualquier texto en
una cadena larga e ilegible de letras y numeros llamado hash(como ejemplo el algoritmo 
sha256).
"""
import hashlib

"""
creamos la clase paciente aplicando POO basica, utilizamos atributos privados
(con doble guion) y getters tradicionales para proteger la informacion,
y sea dificil de modificarse.
"""
class Paciente:
    def __init__(self, id_paciente, nombre, dni):
        self.__id_paciente = id_paciente
        self.__nombre = nombre
        """
        usamos haslib.sha256 para hashear el DNI y cumplir con la lay N.° 29733
        .encode() pasa el texto a bytes y .hexdigest() lo vuelve un codigo seguro
        de letras y numeros, evitando guardar datos muy sensibles en texto plano
        (en pantalla o en la base de datos) y se protege la privacidad.
        """ 
        self.__dni_hash = hashlib.sha256(dni.encode()).hexdigest()
        self.__historial = []

    """
    Getters tradicionales: estos funcionan como unas ventanilla blindada que te 
    permite ver los datos privados desde fuera pero no te deja modificarlos,
    protegiendo los datos sensibles de los pacientes.
    """
    def get_id_paciente(self):
        return self.__id_paciente

    def get_nombre(self):
        return self.__nombre

    def get_dni_hash(self):
        return self.__dni_hash

    def get_historial(self):
        return self.__historial

    """
    el metodo setter o modificador controla las notas medicas y agregar al historial
    del paciente, de forma segura y validada 
    """
    def agregar_nota_historial(self, nota):
        if nota == "":
            raise Exception("La nota médica no puede estar vacía.")
        """
        el append es como agregar un nuevo elemento, en este caso una observacion
        o diagnostico al historial clinico privado del paciente.
        """
        self.__historial.append(nota)


"""
se crea la clase CitaMedica para manejar las citas de los pacientes con los 
medicos del establecimiento de salud, asi todos podran ser atendidos sin 
problemas y de manera  organizada.
"""
class CitaMedica:
    def __init__(self, id_cita, paciente, fecha, medico):
        self.__id_cita = id_cita
        self.__paciente = paciente
        self.__fecha = fecha
        self.__medico = medico
        self.__activa = True

    def get_id_cita(self):
        return self.__id_cita

    def get_paciente(self):
        return self.__paciente

    def get_estado(self):
        return self.__activa
    """
    aqui cuando se crea una cita, la cita ya nace activa por defecto
    """
    def cancelar_cita(self):
        """
        si un paciente o el medico decide anular la cita, el sistema cambia a ese estado
        a 'false' por ello este bloque actua como un filtro de seguridad y revisa si ya estaba
        cancelada.
        """
        if self.__activa == False:
            raise Exception("La cita ya se encontraba cancelada previamente.")
        self.__activa = False


"""
la clase EsSaludManagerSistema se encarga de administrar las colecciones de
datos ( lista de pacientes y citas) aplicando la programacion funcional.
el append
"""
class EsSaludManagerSistema:
    def __init__(self):
        self.lista_pacientes = []
        self.lista_citas = []

    def registrar_paciente(self, paciente):
        """
        aqui el append cuando registra a un nuevo paciente lo coloca al final
        de la lista general llamada lista_pacientes."""
        self.lista_pacientes.append(paciente)

    def programar_cita(self, cita):
        self.lista_citas.append(cita)

    """
    la funcion filter permite crear una nueva lista con los pacientes que 
    cumplen con el criterio de busqueda, y el lambda es uan funcion que ayuda a 
    filtrar los datos de manera mas rapida sin alterar nada, los dos son como una seguridad si cumplen con lo 
    que se busca o no. 
    """
    def buscar_paciente_por_nombre(self, criterio):
        try:
            return list(filter(lambda p: criterio.lower() in p.get_nombre().lower(), self.lista_pacientes))
        except Exception as e:
            print("Error durante la búsqueda:", e)
            return []

    """
    la funcion map permite crear una nueva lista con los pacientes que cumplen con el criterio de busqueda, le 
    aplica una transformacion para generar un reporte limpio y  formateado sin alterar los datos originales."""
    def generar_reporte_pacientes(self):
        return list(map(
            lambda p: f"ID: {p.get_id_paciente()} | Paciente: {p.get_nombre()} | DNI Hash: {p.get_dni_hash()[:10]}...",
            self.lista_pacientes
        ))


"""
aca tenemos el bloques principal de ejecucion del programa , para poner aprueba el sistema 
completo en la consola, mostrando la viabilidad tecnica de todo el codigo.
"""
if __name__ == "__main__":
    print("--- INICIANDO SISTEMA ESSALUD MANAGER ---")
    """
    esto es como encender el programa para que administre las listas vacias
    de pacientes y citas.
    """
    sistema = EsSaludManagerSistema()

    """
    se crea registros de pacientes de prueba
    """
    p1 = Paciente("P001", "Ana Torres", "45678912")
    p2 = Paciente("P002", "Carlos Pérez", "12345678")
    
    sistema.registrar_paciente(p1)
    sistema.registrar_paciente(p2)

    """
    aqui ordenamos al sistema buscar con filter y lambda a cualquier paciente que tenga 
    la palabra 'Ana'
    """
    encontrados = sistema.buscar_paciente_por_nombre("Ana")
    print(f"Pacientes encontrados: {len(encontrados)}")
    """
    aqui es donde el bucle for imprime el nombre del paciente y sus primeros
    12 caracteres del DNI protegido por seguridad.
    """
    for p in encontrados:
        print(f"-> {p.get_nombre()} | DNI Protegido: {p.get_dni_hash()[:12]}...")

    """
    aqui la funcion map trasforma la toda la lista de pacientes en un formato de texto limpio
    y ordenado.
    """
    print("\n--- REPORTE GENERAL ---")
    reporte = sistema.generar_reporte_pacientes()
    """
    al final  el usuario o el medico le hechan un vistazo al reporte
    """
    for linea in reporte:
        print(linea)

    
    #mostramos el encabezamos con un buble infinito para mantener el menu activo hasta 
    #que el usuario decida salir del programa
    
    print("\n--- MENÚ INTERACTIVO DEL SISTEMA ---")
    while True:
        print("\n1. Registrar nuevo paciente")
        print("2. Buscar paciente por nombre")
        print("3. Ver reporte general")
        print("4. Salir")
        
        #capturamos la opcion que el usuario va a ingresar por teclado
        
        opcion = input("Elige una opción (1-4): ")
        
        #si elije la opcion 1, se le solicita sus datos ,para un nuevo objeto paciente y 
        #registrarlo
        if opcion == "1":
            id_p = input("Ingrese ID del paciente (ej. P003): ")
            nombre = input("Ingrese nombre completo: ")
            dni = input("Ingrese DNI (8 dígitos): ")
            nuevo_p = Paciente(id_p, nombre, dni)
            sistema.registrar_paciente(nuevo_p)
            print("¡Paciente registrado y DNI protegido con éxito!")

         #si elije la opcion 2, se usa el filter y el lambda para buscar coincidencias parciales
         #por nombres
        elif opcion == "2":
            criterio = input("Ingrese el nombre a buscar: ")
            encontrados = sistema.buscar_paciente_por_nombre(criterio)
            print(f"Pacientes encontrados: {len(encontrados)}")
            for p in encontrados:
                print(f"-> {p.get_nombre()} | DNI Protegido: {p.get_dni_hash()[:12]}...")

        #si elije la opcion 3, se aplica el map para generar y mostrar el reporte general
        #actualizado
                
        elif opcion == "3":
            print("\n--- REPORTE GENERAL ACTUALIZADO ---")
            reporte = sistema.generar_reporte_pacientes()
            for linea in reporte:
                print(linea)

        #si elije la opcion 4, se rompre el bucle con un break para salir del programa de forma 
        # segura        
        elif opcion == "4":
            print("Saliendo del sistema. ¡Hasta luego!")
            break
        #si ingresa cualquier otra cosa, se muestra un mensaje de advertencia
        else:
            print("Opción no válida. Intente de nuevo.")