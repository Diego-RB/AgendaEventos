import { User } from './User';

export interface Evento {
  id: number;
  local: string;
  dataEvento: Date;
  tema: string;
  descricao: string;
  tipo: string;
  usuarioId: number;
  usuarioEventos: User[];
}
