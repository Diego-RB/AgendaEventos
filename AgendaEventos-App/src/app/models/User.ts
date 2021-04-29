import { Evento } from './Evento';

export interface User {
  id: number;
  nome: string;
  email: string;
  dataNascimento: Date;
  sexo: string;
  senha: string;
  usuarioEvento: Evento[];

}
