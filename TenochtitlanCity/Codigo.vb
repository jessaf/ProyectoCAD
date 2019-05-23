Module Codigo
    Public AUTOCADAPP As AutoCAD.AcadApplication
    Public DOCUMENTO As AutoCAD.AcadDocument
    Public UTILITY As AutoCAD.AcadUtility

    'Public Declare PtrSafe Sub Sleep Lib "kernel32" (ByVal Milliseconds As LongPtr)
    'Public Declare Sub Sleep Lib "kernel32" Alias "Sleep" (ByVal dwMilliseconds As Long)

    ' Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long) 'For 32 Bit Systems  

    Public Sub inicializa_conexion(ByRef version As String)
        Dim R As String = ""

        If version = "2017" Then
            R = "Autocad.Application.21" 'R2019
        ElseIf version = "2018" Then
            R = "Autocad.Application.22" 'R2019
        ElseIf version = "2019" Then
            R = "Autocad.Application.23" 'R2019
        End If

        Try
            DOCUMENTO = Nothing
            AUTOCADAPP = GetObject(, R)                 'Aqui jala el proceso.
            DOCUMENTO = AUTOCADAPP.ActiveDocument       'El documento activo que se esta visualizando en autocad
            UTILITY = DOCUMENTO.Utility
            AUTOCADAPP.Visible = True                   ' Si no se esta visualizando autocad entonces forzamos la visualizacion.
        Catch er As Exception
            MsgBox(er.Message, MsgBoxStyle.Information, "CAD")
        End Try
        'DOCUMENTO es la referencia al documento de ACAD
        'UTILITY 

    End Sub

    Public Sub appactivateAutoCAD()
        'activando AutoCAD para los select
        Dim AUTOCADWINDNAME As String
        Dim acadProcess() As Process = Process.GetProcessesByName("ACAD")
        Try
            'guaradando variables para activar autocad cuando sea necesario
            AUTOCADWINDNAME = acadProcess(0).MainWindowTitle
            AppActivate(AUTOCADWINDNAME)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Function DetenerSimulacion()
        Exit Function
    End Function

    Public Function conjunto_vacio(ByRef DOCUMENTO As AcadDocument, ByRef nombre As String) As AcadSelectionSet

        'Esta funcion no reserva espacio en memoria para un conjunto vacio en el cual meter objetos.
        Dim indice As Integer
        Dim limite As Integer
        Dim cObjects As AcadSelectionSet = Nothing

        nombre = nombre.Trim.ToUpper ' los conjuntos deben ser en mayuscula.
        'conjunto_vacio = Nothing
        Try
            'Autocad tiene conjuntos, aqui hace referencia a los conjuntos de seleccion en el documento acutal. 
            limite = DOCUMENTO.SelectionSets.Count
            limite = limite - 1
            For indice = 0 To limite
                If DOCUMENTO.SelectionSets.Item(indice).Name = nombre Then
                    DOCUMENTO.SelectionSets.Item(indice).Delete()
                    Exit For
                End If
            Next
            cObjects = DOCUMENTO.SelectionSets.Add(nombre)

        Catch ex As ApplicationException
            MsgBox(ex.Message, MsgBoxStyle.Information, "CAD")
        End Try
        Return cObjects  'regresa siempre conj limpio de elementos
    End Function

    Public Sub addXdata(entidad As AcadEntity, nameXrecord As String, valor As String)
        'Agrega un Xrecord y un solo valor
        'Agregar el diccionario al objeto
        'Agregar un registro
        'Ponerle un vlaor
        Dim dictASTI As AcadDictionary
        Dim astiXRec As AcadXRecord
        Dim keyCode() As Short 'Obligadorio que sea short. integer envia  error en el setrecordadata 'llave
        Dim cptData() As Object 'Obligatorio object                         'dato

        Dim getKey As Object = Nothing
        Dim getData As Object = Nothing

        ReDim keyCode(0)
        ReDim cptData(0)

        'Este meto regresa un objeto de tipo diccionario 
        'Ya teniendo el dicicionario se usa addxrecod y ese record se le pone valores y se le asignana valores al registro. se hace con SetXRecordData
        dictASTI = entidad.GetExtensionDictionary                  'accediendo al dicc del elemento
        astiXRec = dictASTI.AddXRecord(nameXrecord.ToUpper.Trim)   'agrega el registro astiXRec es el registro
        'el codigo 100 es de un string
        keyCode(0) = 100 : cptData(0) = valor     'poner valores a los registros
        astiXRec.SetXRecordData(keyCode, cptData) 'asignar los valores a los registros

    End Sub

    Public Sub getXdata(entidad As AcadEntity, nameXrecord As String, ByRef valor As String)
        'extrayendo datos
        'estamos considerando que un Xrecord solo tiene un dato lo cual..
        'Entro al diccionario 
        'Tengo que saber si el diccionario tiene el registro. 

        'agrga un Xrecord y un solo valor
        'Regresa los object como arreglos dinamicos con getxrecorddata.
        Dim astiXRec As AcadXRecord = Nothing
        Dim dictASTI As AcadDictionary
        Dim getKey As Object = Nothing
        Dim getData As Object = Nothing

        valor = Nothing
        dictASTI = entidad.GetExtensionDictionary
        Try
            astiXRec = dictASTI.Item(nameXrecord.ToUpper.Trim) ' Revisando si existe el Xrecord

        Catch ex As Exception
            'No existe el Xrecord
            'MsgBox("No existe el xrecrod")
        End Try

        If Not IsNothing(astiXRec) Then
            'MsgBox("Si existe el xrecrod")
            astiXRec.GetXRecordData(getKey, getData)
            If Not IsNothing(getData) Then
                valor = getData(0) 'recuperando el valor del XRecord
                'MsgBox("Si existe el valor del xrecord y es =" & valor.ToString)
            End If
        End If
    End Sub

    Public Function seguimientoCalle()

        'Para este ejemplo se tienen que crear las Layers en el Banner/Layers/LayerProperties y crear una que se llame LINEAS, CIRCULOS, OTROS
        'revisando todos los elementos del plano
        Dim conjunto As AcadSelectionSet = Nothing
        Dim conjuntoCalles As AcadSelectionSet = Nothing
        Dim conjuntoTemp As AcadSelectionSet = Nothing
        Dim conjuntoVehiculos As AcadSelectionSet = Nothing
        Dim conjuntoCallesEnBorde As AcadSelectionSet = Nothing
        Dim conjuntoSemaforos As AcadSelectionSet = Nothing
        Dim element As AcadEntity
        Dim vehiculo As AcadEntity
        Dim lines() As AcadEntity
        Dim calle As AcadEntity
        Dim temp(0) As AcadEntity
        Dim temp2(0) As AcadEntity
        Dim temp3(0) As AcadEntity
        Dim contorno As AcadEntity = Nothing
        Dim nextStreet As AcadEntity
        Dim lista As ListBox
        Dim FilterType(0) As Integer
        Dim FilterData(0) As Object
        Dim p1() As Double
        Dim p2() As Double
        Dim numMov As Integer = 200
        Dim incX, incY As Double
        Dim currentPosition(0 To 2) As Double
        Dim nextPosition(0 To 2) As Double
        Dim prueba(2) As Object
        Dim intersectionpoints As VariantType
        Dim str As String = Nothing
        Dim I As Integer, j As Integer, k As Integer
        Dim numCalles As Integer = Nothing
        Dim fileName As String, textData As String, textRow As String, fileNo As Integer
        Dim numCarros As Integer

        '===================================================================================
        'PASO 1 Encontrando calles, contornos y demas 
        '===================================================================================

        'ObteniendoInfoDelPlano(conjuntoCalles, conjuntoVehiculos, conjuntoTemp, conjunto, contorno)
        'Creamos los conjuntos con los cuales trabajaremos
        conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
        conjuntoCalles = conjunto_vacio(DOCUMENTO, "Calles")
        conjuntoTemp = conjunto_vacio(DOCUMENTO, "Temp")
        conjuntoVehiculos = conjunto_vacio(DOCUMENTO, "Vehiculos")
        conjuntoCallesEnBorde = conjunto_vacio(DOCUMENTO, "Borde")
        conjuntoSemaforos = conjunto_vacio(DOCUMENTO, "Semaforos")
        conjunto.Select(AcSelect.acSelectionSetAll)


        'Clasificamos en conjuntos los elementos encontrados en el plano
        For Each element In conjunto
            'Obtenemos el contorno
            If element.ObjectName = "AcDbPolyline" Then
                contorno = element
                'MsgBox("Se encontro el contorno")

            End If
            'Obtenemos las calles
            If element.ObjectName = "AcDbLine" Then
                temp2(0) = element
                conjuntoCalles.AddItems(temp2)
                'MsgBox("Se ha agregado elemento a calles")
            End If
            If element.ObjectName = "AcDbCircle" Then
                temp3(0) = element
                conjuntoSemaforos.AddItems(temp3)

            End If
        Next

        '##########################################################
        '##########################################################
        '##########################################################
        '  asignando diccionarios con valores para los semaforo

        MsgBox("numero de semaforos:" & conjuntoSemaforos.Count)

        For Each element In conjuntoSemaforos
            addXdata(element, "LUZ", varColor)
        Next





        '===================================================================================
        'PASO 2 Crear los vehiculos en las calles
        '===================================================================================


        'PRIMERO PROCEDEMOS A CREAR LOS VEHICULOS EN AQUELLAS INTERSECCIONES ENTRE LAS CALLES Y EL CONTORNO
        numCarros = CreandoVehiculos(conjuntoCalles, conjuntoVehiculos, conjuntoCallesEnBorde, contorno, lista)

        '=========================================================================================================
        ''Esta seccion es la que se tiene que tener para que los vehicuos se muevan al mismo tiempo y no uno despues del otro 
        'para que funcone se tiene que modificar la funcion de movimiento para que consulte el diccionario y verifque en que paso esta el vehiculo
        'luego de un paso (un solo movimiento) y mmodifique los valores en el diccionario para acutalizar la informacion acerca del paso actual
        '
        'De esta forma en el ciclo for each aqui abajo, ira iterando por cada carro y por cada carro dara un paso y luego pasara al siguiente carro
        'con esto se lograra dar la sensacion de que todos avanzan simultaneamente 
        '=========================================================================================================

        ''por los siglos de los siglos amen 
        'Do While 1
        '    For Each vehiculo In conjuntoVehiculos

        '        'Obtenemos el paso actual y el numero de pasos a ejecutar
        '        pasoactual = CInt(GetDictionaryVehiculo(vehiculo, "PASOACTUAL", valor))
        '        numsteps = CInt(GetDictionaryVehiculo(vehiculo, "NUMSTEPS", valor))

        '        'Si el paso actual no es el inicial entonces estamos en un punto intermedio de la calle 
        '        If ((pasoactual <> 1) And (pasoactual < numsteps)) Then
        '            moverVehiculo(nextStreet, vehiculo, 200, lista)
        '        End If
        '        'Si el paso actual es uno entonces estamos al comienzo del camino 
        '        'En cuyo caso tenemos que obtener la siguiente calle, girar acorde a ella, y realizar el primer movimiento que nos sacara del inicio del camino 
        '        If pasoactual = 1 Then
        '            nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, numCalles, lista), numCalles)
        '            If Not IsNothing(nextStreet) Then
        '                GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
        '                moverVehiculo(nextStreet, vehiculo, 200, lista)
        '            Else
        '                MsgBox("Saltamos ese vehiculo por no encontrar calle siguiente")
        '            End If
        '        End If

        '    Next

        'Loop

        '=========================================================================================================
        'Esta seccion funciona actualmente 
        '=========================================================================================================

        For Each vehiculo In conjuntoVehiculos
            nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, numCalles, lista), numCalles)
            If Not IsNothing(nextStreet) Then
                GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
                moverVehiculo(nextStreet, vehiculo, 5, lista)
            Else
                MsgBox("Saltamos ese vehiculo por no encontrar calle siguiente")
            End If

        Next





        'conjunto.Delete()
    End Function
    Public Function CreandoVehiculos(conjuntoCalles As AcadSelectionSet, conjuntoVehiculos As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, contorno As AcadEntity, lista As ListBox) As Integer
        'eSTA FUNCION CREA LOS VEHICULOS EN LAS INTERSECCIONES CON EL CONTORNO (POLILINEA)
        Dim calle As AcadEntity
        Dim contadorvehiculos As Integer
        Dim contadorCalles As Integer
        Dim arrcontorno() As Double
        Dim p1() As Double
        Dim p2() As Double
        Dim vehiculo As AcadEntity
        Dim temp(0) As AcadEntity
        Dim tempcallesenborde(0) As AcadEntity
        Dim tempcontadorcallesencontorno As Integer

        contadorvehiculos = 0
        tempcontadorcallesencontorno = 0

        For Each calle In conjuntoCalles

            'lista.Items.Add(calle.ObjectName & " " & "Handler=" & calle.Handle) '
            'element.Layer = "LINEAS"
            p1 = calle.startpoint
            p2 = calle.endpoint

            contadorCalles = contadorCalles + 1

            arrcontorno = Nothing

            'Pregunto si hay interseccion entre la calle y el contorno y si la hay guardo el punto de interseccion en el arreglo arrcontorno
            arrcontorno = contorno.IntersectWith(calle, AcExtendOption.acExtendNone)



            'Si el tamanio del arreglo es mayor a cero entonces tiene las coordenadas guardadas por lo que en efecto hubo interseccion
            'MsgBox("Se han revisado las calles que intesectan con el contorno ")
            If (arrcontorno.Count > 0) Then
                'La siguiente condicion es vital para que los vehiculos desaparezcan de un borde y aparezcan en otro, si no se tiene la siguiente 
                'condicion apareceran carros en medio del plano, esto no es deseable, sin embargo por algun motivo ocurre algo muy raro:
                'En algunos casos La primer condicion se cumple, lo cual significa que se detecta una interseccion entre el contorno y la calle
                'sin embargo cuando en la siguiente condicion se intenta comparar coordenadas, se obtienen coordenadas diferentes (A pesar de que la condicion anterior nos dice que si interceptan)
                'y por lo tanto no detecta bien cuales son las calles que intersectan.
                'Es observable que la diferencia entre las coordenadas de la interseccion y las coordenadas del punto de la linea no son muy grandes, por lo que esto
                'se podria resolver haciendo una resta y poniendo una condicion que testee si la resta es minima. 
                'MsgBox("El tamani del arreglo que guarda las coordenadas es " & arrcontorno.Count & ", " & arrcontorno(0) & "//" & arrcontorno(1) & "                                    Coordenadas de la linea: X=" & p1(0) & "//" & p1(1) & "," & "Y=" & p2(0) & "//" & p2(1))
                'If (((p1(0) = arrcontorno(0) Or p1(0) = arrcontorno(1)) And ((p1(1) = arrcontorno(1)) Or p1(1) = arrcontorno(0))) Or ((p2(0) = arrcontorno(0) Or p2(0) = arrcontorno(1)) And ((p2(1) = arrcontorno(1)) Or p2(1) = arrcontorno(0)))) Then
                tempcallesenborde(0) = calle
                conjuntoCallesEnBorde.AddItems(tempcallesenborde)
                tempcontadorcallesencontorno = tempcontadorcallesencontorno + 1
                'MsgBox("Se encontro nueva calle en el contorno")



                ''''''''''''''Necesito saber si intersecta en el punto inicial o en el punto final de linea
                ''''''''''''''Luego necesito leer el diccionario para saber la direccion de la calle 
                ''''''''''''''En base a eso iria una anidacion de ifs del siguiente estilo
                ''''''''''''''if (intersecta en punto final)
                ''''''''''''''   if (direccion de la calle = izq)
                ''''''''''''''       aparecer carro

                ''''''''''''''if (intersecta en punto inicial)
                ''''''''''''''   if (direccion de la calle = der)
                ''''''''''''''       aparecer carro

                'CREAMOS EL VEHICULO EN EL PUNTO INICIAL DE LA CALLE 
                vehiculo = DOCUMENTO.ModelSpace.AddBox(p1, 2, 1, 2)
                'Le agregamos su respectivo diccionario
                AddDictionaryToVehiculo(vehiculo)
                temp(0) = vehiculo
                conjuntoVehiculos.AddItems(temp)
                contadorvehiculos = contadorvehiculos + 1
                'End If



            End If
            '[PEND} agregar un else con un random que de igual manaera agrege vehiculos aleatoriamente en la parte central del plano 


        Next
        MsgBox("el numero total de calles en el contorno es = " & tempcontadorcallesencontorno)
        Return contadorvehiculos

    End Function

    Public Sub GirarVehiculoSegunOrientacionDeLaCalle(nextStreet As AcadEntity, vehiculo As AcadEntity, lista As ListBox)

        'Le pasamos la nueva calle en la que se encuntra el vehiculo y el vehiculo en cuestion para que haga trigonometria y calcule el angulo que debe tener el 
        'vehiculo para alinearse al angulo de la calle. 

        Dim pendiente As Double
        Dim p1() As Double
        Dim p2() As Double
        Dim ca, co, hip, angle As Double
        Dim valor As String = Nothing
        Dim centroideVehiculo() As Double = Nothing


        '
        If Not IsNothing(nextStreet) Then
            'Calcularemos la pendiente de la siguiente calle
            p1 = nextStreet.startpoint
            p2 = nextStreet.endpoint
        Else
            MsgBox("No se encontro nextstreet en funcion de giro")
        End If


        pendiente = (p2(1) - p1(1)) / (p2(0) - p1(0))
        'Hacemos trigonometria para obtener el angulo 
        ca = (p2(0) - p1(0))
        co = (p2(1) - p1(1))
        hip = Math.Sqrt(Math.Pow(ca, 2) + Math.Pow(co, 2))
        angle = Math.Acos(ca / hip)
        'Ajustamos angle para dar el giro correcto dependiendo de la pendiente de la calle 
        If pendiente < 0 Then
            angle = angle * -1
        End If

        'Obtenemos el valor del augulo anterior para poder determinar como girar
        'Notese que no importa si el angulo anterior fue negativo o positivo, se ajustara hacia la direccion contraria 
        'gracias a la multiplicacion por -1
        GetDictionaryVehiculo(vehiculo, "ANGULOACTUAL", valor)
        If valor <> "VACIO" Then
            'anguloactual = CDbl(valor)
            vehiculo.Rotate(p1, CDbl(valor) * -1)
        End If

        vehiculo.Rotate(p1, angle)
        UpdateDictionaryToVehiculo(vehiculo, angle)

    End Sub

    Public Function moverVehiculo(calle As AcadEntity, vehiculo As AcadEntity, numMov As Integer, lista As ListBox)

        'Esta funcion es la que se tiene que modificar para ir avanzando paso a paso en concordancia con el codigo comentado de la funcion seguimientoCalle()


        Dim p1(), p2() As Double
        Dim incX, incY As Double
        Dim currentPosition(0 To 2) As Double
        Dim nextPosition(0 To 2) As Double
        Dim centroideVehiculo() As Double = Nothing

        p1 = calle.startpoint
        p2 = calle.endpoint

        'Se establece un incremento a partir de el numero de movimientos que se deseen para el vehiculo
        'Esto tiene el inconveniente de que entre mas grande sea la calle mas grande seran los saltos que vaya
        'dando el vehiculo. 
        incX = (p2(0) - p1(0)) / numMov
        incY = (p2(1) - p1(1)) / numMov


        'Se guarda la posicion actual
        currentPosition(0) = p1(0)
        currentPosition(1) = p1(1)

        For count = 1 To numMov


            'Se mueve el vehiculo a la siguiente posicion
            If (count = numMov) Then
                vehiculo.Move(currentPosition, p2)
            Else
                'la siguiente posicion consistira  en la posicion actual mas un incremento
                nextPosition(0) = currentPosition(0) + incX
                nextPosition(1) = currentPosition(1) + incY

                vehiculo.Move(currentPosition, nextPosition)

                currentPosition(0) = nextPosition(0)
                currentPosition(1) = nextPosition(1)

                AnalizandoEntornoCircular(currentPosition) 'analiza lo que hay en la nueva posicion a la que llega
                MsgBox("analizo")


            End If
            vehiculo.Update()

            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & currentPosition(0) & " // " & currentPosition(1)) '
            'WasteTime(1)
            'PauseEvent(1)
        Next

    End Function

    Public Sub AnalizandoEntornoCircular(p() As Double)
        '###  Funcion que analiza dentro de un entorno circular los objetos que se encuentran al rededor del vehículo
        'Esta funcion se manda a llamar cada vez que se actualiza la posicion del vehiculo

        Dim conjunto As AcadSelectionSet
        'Dim delta As Double = 500
        Dim esquinas(11) As Double
        Dim lista As ListBox = Form1.ListBox1 'para mostrar en un list box los objetos que fueron encontrados dentro del área analizada
        Dim perimetro As AcadPolyline = Nothing
        Dim radio As Double

        'appactivateAutoCAD()
        radio = 3.0

        If Not IsNothing(p) Then

            esquinas = generaCoordenadasCirculos(p, radio, 0, 360, 20)
            perimetro = drawPolygon(esquinas) 'trazando el poligono de busqueda

            conjunto = conjunto_vacio(DOCUMENTO, "Crossing elements")
            conjunto.SelectByPolygon(AcSelect.acSelectionSetCrossingPolygon, esquinas)

            lista.Items.Clear()

            For Each element In conjunto
                'no reportamos el perimetro generado
                If element.handle <> perimetro.Handle Then
                    If element.ObjectName <> "AcDb3dSolid" Then ' se exluye el auto 
                        lista.Items.Add(element.handle & " " & element.ObjectName) 'muestra en la lista los objetos encontrado
                        '####################
                        'hacer condiciones de los semaforos obteniendo los valores de los diccionario 
                        ' if valor del luz del dicc == roja
                        '   detenerse x tiempo


                    End If
                End If
            Next
            conjunto.Delete()
        End If
        perimetro.Delete()

    End Sub

    Public Function generaCoordenadasCirculos(p() As Double, radio As Double, angInicial As Double, angFinal As Double, avances As Integer) As Double()
        'grados estan Angulos esta en grados
        'Debe regresar un arreglo lineal donde cada 3 elementos son una coordenada

        Dim angulo As Double
        Dim anguloDeAvance As Double
        Dim pCirculo() As Double
        Dim pPolar() As Double
        Dim pN As Integer
        'Dim angFinal As Double

        pN = 0

        anguloDeAvance = convertAngtoRad(angFinal - angInicial) / avances 'radianes
        angulo = convertAngtoRad(angInicial)
        For i = 1 To avances
            pPolar = DOCUMENTO.Utility.PolarPoint(p, angulo, radio)
            'Una coordenada polar requiere estos datos. Se avanca el numero de grados dividido entre el numero de puntos que tengo,. Esto se pudo hacer por seno y coseno pero se prefirio hacerlo así. 
            ReDim Preserve pCirculo(pN + 2)
            pCirculo(pN) = pPolar(0) : pCirculo(pN + 1) = pPolar(1) : pCirculo(pN + 2) = 0
            'Esto es un arreglo dinamico de coordenadas. 
            angulo = angulo + anguloDeAvance
            pN = pN + 3
        Next
        Return pCirculo

    End Function

    Public Function convertAngtoRad(anguloGrados As Double) As Double
        Return (anguloGrados * 3.1416 / 180.0)
    End Function

    Public Function drawPolygon(ByVal coordenadas() As Double) As AcadPolyline
        'genera un poligono en el modelspace
        Dim perimetro As AcadPolyline
        Dim uB As Integer
        Dim uI As Integer
        uI = coordenadas.GetUpperBound(0)
        'redimensionando el arreglo dinamico para que acepte una coordenada adicional
        uB = uI + 3
        ReDim Preserve coordenadas(uB)

        'agregagndo nuevaente la primera coordenada para generar un poligono cerrado
        coordenadas(uI + 1) = coordenadas(0)
        coordenadas(uI + 2) = coordenadas(1)
        coordenadas(uI + 3) = coordenadas(2)

        'crando el poligono cerrado
        perimetro = DOCUMENTO.ModelSpace.AddPolyline(coordenadas)
        perimetro.Update()
        Return perimetro

    End Function

    Public Sub AddDictionaryToVehiculo(vehiculo As AcadEntity, origin As Double(), destiny As Double(), steps As Integer, handle As Integer, angle As Double, position As Double())
        'Esta funcion se disenio para actualizar todos los datos del diccionario del vehiculo pero hasta el momento no ha surgido la necesidad de actualiza todos los datos simultaneamente
        If Not IsNothing(vehiculo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ORIGEN_X", Str(origin(0)))
            addXdata(vehiculo, "ORIGEN_Y", Str(origin(1)))
            addXdata(vehiculo, "DESTINO_X", Str(destiny(0)))
            addXdata(vehiculo, "DESTINO_Y", Str(destiny(1)))
            addXdata(vehiculo, "NUMSTEPS", Str(steps))
            addXdata(vehiculo, "HANDLE", Str(handle))
            addXdata(vehiculo, "ANGULOACTUAL", Str(angle))
            addXdata(vehiculo, "POSICIONACTUAL_X", Str(position(0)))
            addXdata(vehiculo, "POSICIONACTUAL_Y", Str(position(1)))
            addXdata(vehiculo, "START", "FALSE")
            'MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")
        End If
    End Sub

    Public Sub AddDictionaryToVehiculo(vehiculo As AcadEntity)
        'Esta funcion inicializa el diccionario del vehiculo con valores vacios
        'NOTESE QUE HAY VALORES EN DONDE SE PUEDE GUARDAR LA X Y Y ACTUALES, EL NUMERO DE PASO ACUTAL, EL NUMERO DE PASOS A SEGUIR Y EL DESTINO, CON ESOS DATOS SE PUEDE IR 
        'AVANZANDO PASO A PASO DESPUES DE MODIFICAR LA FUNCION DE MOVIMIENTO SEGUN LO QUE SE INDICA EN LOS COMENTARIOS DE LA FUNCION seguimientoCalles()

        If Not IsNothing(vehiculo) Then

            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ORIGEN_X", "VACIO")
            addXdata(vehiculo, "ORIGEN_Y", "VACIO")
            addXdata(vehiculo, "DESTINO_X", "VACIO")
            addXdata(vehiculo, "DESTINO_Y", "VACIO")
            addXdata(vehiculo, "NUMSTEPS", "0")
            addXdata(vehiculo, "HANDLE", "VACIO")
            addXdata(vehiculo, "ANGULOACTUAL", "VACIO")
            addXdata(vehiculo, "POSICIONACTUAL_X", "VACIO")
            addXdata(vehiculo, "POSICIONACTUAL_Y", "VACIO")
            addXdata(vehiculo, "START", "TRUE")
            addXdata(vehiculo, "PASOACTUAL", "1")
            addXdata(vehiculo, "X_ACTUAL", "VACIO")
            addXdata(vehiculo, "Y_ACTUAL", "VACIO")


            'MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")

        End If
    End Sub
    Public Sub UpdateDictionaryToVehiculo(vehiculo As AcadEntity, angle As Double)
        'ESTA FUNCION ES EXTREMADAMENTE UTIL POR QUE NOS PERMITE ACTUALIZAR EL VALOR DE UN DATO DEL DICCIONARIO, ESTA ACTUALIZACION SE USA POR EJEMPLO EN LA FUNCION DE GIRO AL ACTUALIZAR EL ANGULO DEL VEHICULO
        'ESTA FUNCION SERA NECESARIA PARA ACTUALIZAR LOS DATOS DURANTE CADA PASO DEL MOVIMIENTO. eSTOS PASOS NECESITAN SERA ACTUALIZADOS PARA QUE LA FUNCION DE MOVIMIENT SEPA EN QUE PASO ESTAMOS Y POR LO TANDO DAR EL SIGUIENTE
        If Not IsNothing(vehiculo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ANGULOACTUAL", Str(angle))
        End If
    End Sub

    Public Function GetDictionaryVehiculo(vehiculo As AcadEntity, llave As String, ByRef valor As String)
        'ESTA FUNCION SIMILAR A LA ANTERIOR PERO EN LUGAR DE MODIFICAR EL DATO DEL DICCIONARIO LO CONSULTA, SE SUELEN USAR JUNTAS AMBAS FUCIONESS
        Dim valorretorno As String = Nothing
        If Not IsNothing(vehiculo) Then
            getXdata(vehiculo, llave, valor)
            valorretorno = valor
        End If
        Return valorretorno
    End Function

    Public Function getNextStreet(callesdeinterseccion As Collection, numCalles As Integer) As AcadEntity

        'Esta funcion toma todas las calles de interseccion, y aleatoriamente selecciona una para ser la siguiente calle en la trayectoria
        'LAS CALLES DE LA INTERSECCION SE OBTIENEN DE LA FUNCION SIGUIENTE getNextPosibleStreets.
        Dim randomnumber As Integer
        Randomize()
        randomnumber = Int((callesdeinterseccion.Count - 1 + 1) * Rnd() + 1)


        MsgBox("getnextstreet: RANDOM = " & randomnumber & "," & "tamofcollection = " & callesdeinterseccion.Count)
        If callesdeinterseccion.Count <> 0 Then
            Return callesdeinterseccion.Item(randomnumber)
            MsgBox("Getnextstreet: se procede a ir a la calle numero " & randomnumber & ", de " & numCalles & " posibles")
        Else
            MsgBox("Aqui muere")
        End If

    End Function

    Public Function getNextPosibleStreets(vehiculo As AcadEntity, conjuntoCalles As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, numCalles As Integer, lista As ListBox) As Collection
        'Esta funcion busca cuales son las calles que colindan con la interseccion en la que se encuentra nuestro vehiculo de forma tal que posteriormente se pueda decidir a cual de ellas ir 
        Dim calle As AcadEntity
        Dim p1calle() As Double
        Dim p2calle() As Double
        Dim centroide() As Double
        Dim callesenlainterseccion As Collection = Nothing
        callesenlainterseccion = New Collection

        centroide = vehiculo.centroid
        numCalles = 0

        For Each calle In conjuntoCalles
            p1calle = calle.startpoint
            p2calle = calle.endpoint
            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & centroide(0) & " // " & centroide(1))
            'lista.Items.Add("calle= " & calle.Handle & " p1 = " & p1calle(0) & " // " & p1calle(1) & " p2 = " & p2calle(0) & " // " & p2calle(1))
            If centroide(0) = p1calle(0) And centroide(1) = p1calle(1) Then
                callesenlainterseccion.Add(calle)
                numCalles = numCalles + 1
            End If
        Next
        MsgBox("Se encontraron posibles calles para avanzar #calles = " & callesenlainterseccion.Count)

        If callesenlainterseccion.Count = 0 Then
            callesenlainterseccion.Clear()
            MsgBox("Se llego a una condicion de fin de calle, se procede a reaparecer el vehiculo")
            callesenlainterseccion.Add(reaparecerVehiculo(vehiculo, conjuntoCallesEnBorde))
        End If

        Return callesenlainterseccion
    End Function

    Public Function reaparecerVehiculo(vehiculo As AcadEntity, conjuntoCallesEnBorde As AcadSelectionSet) As AcadEntity

        'Cuando el vehiculo llega al borde de la calle ya no tiene mas calles para seguir avanzando, asi que se selecciona una calle, asi que debemos desaparecerlo de ahi y reaparecerlo en el comienzo de otra calle
        'de las que colindan con el borde aleatoriamente para desaparecer al vehiculo de donde esta (el punto sin avance) y aparecerlo
        'en la nueva calle, esta calle sera la siguiente calle en la cual circulara el vehiculo

        Dim numerodecalles As Integer
        Dim calle As AcadEntity
        Dim randomnumber As Integer
        Dim p1() As Double = Nothing


        numerodecalles = conjuntoCallesEnBorde.Count
        MsgBox("El numero de calles en el borde es = " & numerodecalles)

        If numerodecalles <> 0 Then
            Randomize()
            randomnumber = Int((numerodecalles - 1 + 1) * Rnd() + 1)
            MsgBox("La calle a la que se ira sera la siguiente = " & randomnumber)
            calle = conjuntoCallesEnBorde(randomnumber)
            If Not IsNothing(calle) Then
                p1 = calle.startpoint
                vehiculo.Move(vehiculo.centroid, p1)
                MsgBox("Se pudo reaparecer en la siguiente calle")
            Else
                MsgBox("No se recupero calle")
            End If


            Return calle
        Else
            MsgBox("No se pudo reaparecer en la siguiente calle")
            'Lo siguiente no esta probado, no se sabe como reaccionara el resto del codigo\
            'Si se llega hasta aqui lo mas probable es que el programa crashee por una exepcion 
            'conjuntoCallesEnBorde.RemoveItems(vehiculo)
            'vehiculo.Delete()

        End If

    End Function

End Module