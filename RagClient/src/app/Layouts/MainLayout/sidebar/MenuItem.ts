import { Role } from '../../../BussinessLogic/Helpers/Constants';

export interface MenuItem {
  icon: string;
  title: string;
  link?: string;
  tooltip?: string;
  items?: MenuItem[];
}
