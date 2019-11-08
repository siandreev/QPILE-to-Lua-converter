

grammar QPILE_converterV2;

options
{
	language = CSharp;
}
@parser::namespace { Generated }
@lexer::namespace  { Generated }

program
   : declarationBlock (NEWLINE)* programBlock (NEWLINE)* paramsBlock (NEWLINE)* END_PORTFOLIO_EX (NEWLINE)*
   ;

declarationBlock
    : (PORTFOLIO_EX longName (NEWLINE)* ) (DESCRIPTION longName (NEWLINE)*) (CLIENTS_LIST ALL_CLIENTS SEMI (NEWLINE)*) (FIRMS_LIST (ALL_FIRMS | FIRM_ID) SEMI (NEWLINE)*)
    ;

longName
    : identifier (identifier)* SEMI
    ;

identifier
   : IDENT
   ;

paramsBlock
    : (PARAMETER identifier SEMI (NEWLINE)* ) (PARAMETER_TITLE longName (NEWLINE)*) (PARAMETER_DESCRIPTION longName (NEWLINE)*) (PARAMETER_TYPE (NUMERIC_PARAMTYPE | STRING_PARAMTYPE) SEMI (NEWLINE)*) END
    ;

programBlock
    : PROGRAM (NEWLINE)+ statementList (NEWLINE)+ END_PROGRAM
    ;

statementList
   : statement (NEWLINE statement)*
   ;

statement
    : name EQUAL expression 
    | ifOperator
    | forOperator
    | funcDescr
	| procedureCall
    | CONTINUE
    | BREAK
    | RETURN
    ;

procedureCall
	: fName LPAREN argList1 RPAREN
	| qProcedureCall
	;

qProcedureCall
	: MESSAGE LPAREN argList1 RPAREN
	| DELETE_ALL_ITEMS LPAREN RPAREN
	| ADD_ITEM LPAREN number COMA name RPAREN
	;

ifOperator
    : IF condition NEWLINE
     statementList
     ELSE NEWLINE
     statementList
     END_IF
    ;


condition
    : condition OR condition
    | condition AND condition
    | LPAREN condition RPAREN
    | primaryCondition
    ;

primaryCondition
    : expression (EQUAL | LE | LT | GE | GT | NOT_EQUAL) expression
    ;

forOperator
    : FOR name IN argList NEWLINE
      statementList
      END_FOR
    | FOR name FROM expression TO expression NEWLINE
      statementList
      END_FOR
    ;

argList
    : STRING_SYMBOLS  
    ;

argList1
    : expression
    | argList1 COMMA expression
    ;
        
funcDescr
    : FUNC name LPAREN fargList RPAREN NEWLINE
      statementList NEWLINE
      END_FUNC
    ;

fargList
    : name
    | fargList COMMA name
    ;

expression
    : expression PLUS term
    | expression MINUS term
    | expression COMPOUND term
    | term
    ;

term
    : term SLASH primary
    | term STAR primary
    | primary
    ;

primary
    : number
	| name
    | STRING_SYMBOLS
    | MINUS primary
    | LPAREN expression RPAREN
    | functionCall
    ;

functionCall
    : fName LPAREN argList1 RPAREN
	| qFunctionCall
    ;

qFunctionCall
	: CREATE_MAP LPAREN RPAREN
	| SET_VALUE LPAREN name COMMA STRING_SYMBOLS COMMA expression RPAREN
	| GET_INFO_PARAM LPAREN STRING_SYMBOLS RPAREN
	| SUBSTR LPAREN (name | STRING_SYMBOLS) COMMA number COMMA number RPAREN
	;

fName
    : name
    ;

number
    : NUM_INT
    | NUM_REAL
    ;

name
    : IDENT
    ;











fragment A
   : ('a' | 'A')
   ;


fragment B
   : ('b' | 'B')
   ;


fragment C
   : ('c' | 'C')
   ;


fragment D
   : ('d' | 'D')
   ;


fragment E
   : ('e' | 'E')
   ;


fragment F
   : ('f' | 'F')
   ;


fragment G
   : ('g' | 'G')
   ;


fragment H
   : ('h' | 'H')
   ;


fragment I
   : ('i' | 'I')
   ;


fragment J
   : ('j' | 'J')
   ;


fragment K
   : ('k' | 'K')
   ;


fragment L
   : ('l' | 'L')
   ;


fragment M
   : ('m' | 'M')
   ;


fragment N
   : ('n' | 'N')
   ;


fragment O
   : ('o' | 'O')
   ;


fragment P
   : ('p' | 'P')
   ;


fragment Q
   : ('q' | 'Q')
   ;


fragment R
   : ('r' | 'R')
   ;


fragment S
   : ('s' | 'S')
   ;


fragment T
   : ('t' | 'T')
   ;


fragment U
   : ('u' | 'U')
   ;


fragment V
   : ('v' | 'V')
   ;


fragment W
   : ('w' | 'W')
   ;


fragment X
   : ('x' | 'X')
   ;


fragment Y
   : ('y' | 'Y')
   ;


fragment Z
   : ('z' | 'Z')
   ;

fragment UNDERSCORE
   : '_'
   ;

/* ------------LEXER RULES------------*/

LINE_COMMENT:   '\''
                ~[\r\n]*
                ('\r'? '\n' | EOF)
                              -> channel(HIDDEN); 

STRING_SYMBOLS
   : '"' (~('"'))* '"'
   ;

/* data types description*/

NUMERIC_PARAMTYPE
    : N U M E R I C LPAREN NUM_INT COMMA NUM_INT RPAREN
    ;
      
STRING_PARAMTYPE
    : S T R I N G LPAREN NUM_INT RPAREN
    ;


/*service words description*/

PROGRAM
    : P R O G R A M;

END_PROGRAM
    : E N D UNDERSCORE P R O G R A M;

PORTFOLIO_EX
    : P O R T F O L I O UNDERSCORE E X;

END_PORTFOLIO_EX
    : E N D UNDERSCORE P O R T F O L I O UNDERSCORE E X;

DESCRIPTION
    : D E S C R I P T I O N (' ')*;

CLIENTS_LIST
    : C L I E N T S UNDERSCORE L I S T;

ALL_CLIENTS
    : A L L UNDERSCORE C L I E N T S;

FIRMS_LIST
    : F I R M S UNDERSCORE L I S T;

ALL_FIRMS
    : A L L UNDERSCORE F I R M S;

FIRM_ID
    : F I R M UNDERSCORE I D;

INCLUDE
    : I N C L U D E;

USE_CASE_SENSITIVE_CONSTANTS
    : U S E UNDERSCORE C A S E UNDERSCORE S E N S I T I V E UNDERSCORE C O N S T A N T S;

PARAMETER
    : P A R A M E T E R;

PARAMETER_TITLE
    : P A R A M E T E R UNDERSCORE T I T L E;

PARAMETER_DESCRIPTION 
    : P A R A M E T E R UNDERSCORE D E S C R I P T I O N ;

PARAMETER_TYPE
    : P A R A M E T E R UNDERSCORE T Y P E;
     
AND
    : A N D;

OR 
    : O R;


/*operators*/



END_IF
    : E N D ' ' I F;

IF
    : I F;

ELSE
    : E L S E;

END_FOR
    : E N D ' ' F O R;

FOR
    : F O R;

IN
    : I N;

FROM
    : F R O M;

TO
    : T O;

END_FUNC
    : E N D ' ' F U N C 
    ;

FUNC
    : F U N C;

END
    : E N D;

CONTINUE
    : C O N T I N U E;

BREAK
    : B R E A K;

RETURN
    : R E T U R N;

/*make QPILE embedded functions */

NEW_GLOBAL
    : N E W UNDERSCORE G L O B A L;

MESSAGE
    : M E S S A G E;

CREATE_MAP
	: C R E A T E UNDERSCORE M A P
	;

SET_VALUE
	: S E T UNDERSCORE V A L U E
	;

GET_INFO_PARAM
	: G E T UNDERSCORE I N F O UNDERSCORE P A R A M
	;

SUBSTR
	: S U B S T R
	;
ABS
    : A B S;

ACOS
    :A C O S;

ASIN
    : A S I N;

ATAN
    : A T A N;

CEIL:
        C E I L;

COS
    : C O S;

EXP
    : E X P;

FLOOR
    : F L O O R;

LOG
    : L O G;

POW
    : P O W;

RAND
    : R A N D;

RANDOMIZE
    : R A N D O M I Z E;

SIN
    : S I N;

SQRT
    : S Q R T;

TAN
    : T A N;

GET_ITEM
    : G E T UNDERSCORE I T E M;

GET_NUMBER_OF
    : G E T UNDERSCORE N U M B E R UNDERSCORE O F;


DELETE_ALL_ITEMS
	: D E L E T E UNDERSCORE A L L UNDERSCORE I T E M S;

ADD_ITEM
	: A D D UNDERSCORE I T E M;
/*make data types*/


IDENT
   : ('a' .. 'z' | 'A' .. 'Z') ('a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_')*
   ;
   

NUM_INT
   : ('0' .. '9') +
   ;
   

NUM_REAL
   : ('0' .. '9') + (('.' ('0' .. '9') + (EXPONENT)?)? | EXPONENT)
   ;

fragment EXPONENT
   : ('e') ('+' | '-')? ('0' .. '9') +
   ;

/* other signs*/

PLUS
   : '+'
   ;


MINUS
   : '-'
   ;


STAR
   : '*'
   ;


SLASH
   : '/'
   ;


COMMA
   : ','
   ;


SEMI
   : ';'
   ;
    
ASSIGN
    : ':='
    ;

EQUAL
   : '='
   | '=='
   ;
   


NOT_EQUAL
   : '<>'
   | '!='
   ;


LT
   : '<'
   ;


LE
   : '<='
   ;


GE
   : '>='
   ;


GT
   : '>'
   ;


LPAREN
   : '('
   ;


RPAREN
   : ')'
   ;

COMPOUND
   : '&'
   ;



NEWLINE: '\r'? '\n';

SPACE:          [ \t\r\n]+    -> skip;

